#!/usr/bin/env bash
set -euo pipefail

BASE_REF="${1:-origin/main}"

if ! git rev-parse --git-dir >/dev/null 2>&1; then
  echo "This script must be run inside a git repository." >&2
  exit 1
fi

if ! git rev-parse --verify "$BASE_REF" >/dev/null 2>&1; then
  echo "Base ref '$BASE_REF' was not found."
  echo "Tip: run 'git fetch origin main' or pass a valid ref as the first argument."
  exit 1
fi

RANGE="${BASE_REF}...HEAD"
CHANGED_FILES=$(git diff --name-only "$RANGE")
DIFF_CONTENT=$(git diff "$RANGE")

if [[ -z "$CHANGED_FILES" ]]; then
  echo "No changes detected in range $RANGE."
  exit 0
fi

echo "== Local PR review for $RANGE =="
echo "Changed files:"
printf '%s\n' "$CHANGED_FILES" | sed 's/^/ - /'

FAILED=0
WARNINGS=0

run_check() {
  local title="$1"
  shift
  echo
  echo "[CHECK] $title"
  if "$@"; then
    echo "[PASS] $title"
  else
    echo "[FAIL] $title"
    FAILED=1
  fi
}

run_warning_check() {
  local title="$1"
  shift
  echo
  echo "[CHECK] $title"
  if "$@"; then
    echo "[PASS] $title"
  else
    echo "[WARN] $title"
    WARNINGS=$((WARNINGS + 1))
  fi
}

check_no_conflict_markers() {
  if printf '%s\n' "$DIFF_CONTENT" | rg -n '^(\+|\-)<<<<<<<|^(\+|\-)=======|^(\+|\-)>>>>>>>' >/dev/null; then
    echo "Merge conflict markers detected in the diff."
    return 1
  fi
  return 0
}

check_git_diff_sanity() {
  git diff --check "$RANGE"
}

check_debug_logs() {
  local has_cs_changes
  has_cs_changes=$(printf '%s\n' "$CHANGED_FILES" | rg '\.cs$' || true)

  if [[ -z "$has_cs_changes" ]]; then
    echo "No C# files changed; skipping."
    return 0
  fi

  local suspicious
  suspicious=$(git diff "$RANGE" -- '*.cs' | rg -n '^\+.*(Debug\.Log|Console\.WriteLine\()' || true)
  if [[ -n "$suspicious" ]]; then
    echo "Potential leftover debug logging found:"
    printf '%s\n' "$suspicious"
    return 1
  fi

  return 0
}

check_ai_review_with_ollama() {
  if ! command -v ollama >/dev/null 2>&1; then
    echo "ollama not installed; skipping local AI review."
    return 1
  fi

  local model
  model="${OLLAMA_REVIEW_MODEL:-llama3.1:8b}"

  if ! ollama list | rg -F "$model" >/dev/null 2>&1; then
    echo "Model '$model' not found in Ollama; skipping local AI review."
    echo "Install with: ollama pull $model"
    return 1
  fi

  echo "Running local AI review with Ollama model '$model'..."
  printf '%s\n' "$DIFF_CONTENT" | ollama run "$model" \
    "You are a strict pull-request reviewer. Review this diff and output:\n1) blocking issues\n2) non-blocking suggestions\n3) risk summary.\nBe concise and concrete." || return 1

  return 0
}

check_ai_review_with_openrouter() {
  if [[ -z "${OPENROUTER_API_KEY:-}" ]]; then
    echo "OPENROUTER_API_KEY not set; skipping OpenRouter AI review."
    return 1
  fi

  if ! command -v curl >/dev/null 2>&1; then
    echo "curl is not installed; skipping OpenRouter AI review."
    return 1
  fi

  if ! command -v jq >/dev/null 2>&1; then
    echo "jq is not installed; skipping OpenRouter AI review."
    return 1
  fi

  local model site_url site_name prompt payload response
  model="${OPENROUTER_MODEL:-openai/gpt-4o-mini}"
  site_url="${OPENROUTER_SITE_URL:-https://github.com/local/pr-review}"
  site_name="${OPENROUTER_SITE_NAME:-local-pr-reviewer}"

  prompt=$(cat <<PROMPT
You are a strict pull-request reviewer.
Review the following git diff and output:
1) blocking issues
2) non-blocking suggestions
3) risk summary
Keep it concise and concrete.

Diff:
$DIFF_CONTENT
PROMPT
)

  payload=$(jq -n \
    --arg model "$model" \
    --arg prompt "$prompt" \
    '{
      model: $model,
      messages: [
        {role: "system", content: "You provide strict but practical code reviews."},
        {role: "user", content: $prompt}
      ],
      temperature: 0.2
    }')

  echo "Running AI review with OpenRouter model '$model'..."
  response=$(curl -sS https://openrouter.ai/api/v1/chat/completions \
    -H "Authorization: Bearer ${OPENROUTER_API_KEY}" \
    -H "Content-Type: application/json" \
    -H "HTTP-Referer: ${site_url}" \
    -H "X-Title: ${site_name}" \
    -d "$payload") || return 1

  if printf '%s' "$response" | jq -e '.error' >/dev/null; then
    echo "OpenRouter API returned an error:"
    printf '%s\n' "$response" | jq -r '.error.message // .error'
    return 1
  fi

  printf '%s\n' "$response" | jq -r '.choices[0].message.content // empty'
  return 0
}

check_ai_review() {
  local provider
  provider="${AI_REVIEW_PROVIDER:-auto}"

  case "$provider" in
    openrouter)
      check_ai_review_with_openrouter
      ;;
    ollama)
      check_ai_review_with_ollama
      ;;
    auto)
      if [[ -n "${OPENROUTER_API_KEY:-}" ]]; then
        check_ai_review_with_openrouter
      else
        check_ai_review_with_ollama
      fi
      ;;
    *)
      echo "Unknown AI_REVIEW_PROVIDER '$provider'. Use: auto | openrouter | ollama"
      return 1
      ;;
  esac
}

run_check "No merge conflict markers" check_no_conflict_markers
run_check "No whitespace or unresolved patch issues" check_git_diff_sanity
run_check "No obvious leftover debug logs in C# changes" check_debug_logs
run_warning_check "Optional AI review (OpenRouter/Ollama)" check_ai_review

echo
if [[ "$FAILED" -ne 0 ]]; then
  echo "Local PR review failed. Fix blocking issues before pushing."
  exit 1
fi

echo "Local PR review passed with $WARNINGS warning(s)."
exit 0
