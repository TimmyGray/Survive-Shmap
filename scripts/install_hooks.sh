#!/usr/bin/env bash
set -euo pipefail

git config core.hooksPath .githooks
echo "Git hooks installed. pre-push will now run local PR review."
