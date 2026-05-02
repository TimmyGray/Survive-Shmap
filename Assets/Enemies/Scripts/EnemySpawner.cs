using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main spawner class that manages when and where enemies spawn based on EnemySpawnData configurations.
///
/// HOW IT WORKS:
/// This MonoBehaviour runs every frame (Update()) and checks if it's time to spawn enemies.
/// For each enemy type (defined in enemySpawnData list), it tracks:
/// - When the next spawn should happen
/// - How many have been spawned total
/// - Which enemies are currently alive
///
/// DESIGN PATTERN:
/// This uses a "data-driven" approach - the spawner logic is separate from spawn data.
/// Designers can create different EnemySpawnData assets without touching code.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    /// <summary>
    /// List of all enemy types and their spawn configurations.
    /// Each EnemySpawnData defines one type of enemy and how it should spawn.
    ///
    /// WHY List?
    /// Allows multiple enemy types to be configured in the Unity Inspector.
    /// You can have easy enemies, hard enemies, bosses, etc., all managed by one spawner.
    ///
    /// WHY [SerializeField]?
    /// Makes private fields visible in Unity Inspector so designers can configure them
    /// without needing to make the field public (which breaks encapsulation).
    ///
    /// HOW TO USE:
    /// In Unity Inspector, click the "+" button to add EnemySpawnData assets to this list.
    /// Each entry defines a different enemy type with its own spawn rules.
    /// </summary>
    [Header("Spawn Configuration")]
    [SerializeField]
    private List<EnemySpawnData> enemySpawnData = new List<EnemySpawnData>();

    /// <summary>
    /// Master switch to enable/disable all enemy spawning.
    /// When false, Update() returns early and no enemies spawn.
    ///
    /// WHY this flag?
    /// Useful for pausing spawning during cutscenes, game over, or menu screens.
    /// Can be toggled via SetSpawnEnabled() method from other scripts.
    ///
    /// Example: When player dies, call SetSpawnEnabled(false) to stop spawning.
    /// </summary>
    [Header("Global Settings")]
    [SerializeField]
    private bool spawnEnabled = true;

    /// <summary>
    /// Transform that will be the parent of all spawned enemies in the hierarchy.
    ///
    /// WHY use a container?
    /// Keeps the scene hierarchy organized. Instead of enemies cluttering the root,
    /// they're grouped under "EnemyContainer" making it easier to:
    /// - Find enemies in the hierarchy
    /// - Disable/enable all enemies at once
    /// - Clean up when needed
    ///
    /// HOW TO SETUP:
    /// Create an empty GameObject called "EnemyContainer" and assign it here.
    /// All spawned enemies will be children of this object.
    /// </summary>
    [SerializeField]
    private Transform enemyContainer;

    /// <summary>
    /// Dictionary that tracks spawn state for each enemy type.
    /// Key: The EnemySpawnData (enemy type configuration)
    /// Value: SpawnTracker (tracks timing and counts for that enemy type)
    ///
    /// WHY Dictionary?
    /// Provides fast O(1) lookup - given an EnemySpawnData, instantly find its tracker.
    /// More efficient than searching through a list every frame.
    ///
    /// WHY separate tracker per enemy type?
    /// Each enemy type has independent spawn timing. Easy enemies might spawn every 2s,
    /// bosses every 30s. Each needs its own "next spawn time" tracking.
    ///
    /// HOW IT WORKS:
    /// When we check if an enemy should spawn, we look up its tracker to see:
    /// - Is it time to spawn? (nextSpawnTime)
    /// - How many are alive? (aliveEnemies.Count)
    /// - How many spawned total? (totalSpawned)
    /// </summary>
    private Dictionary<EnemySpawnData, SpawnTracker> spawnTrackers =
        new Dictionary<EnemySpawnData, SpawnTracker>();

    /// <summary>
    /// Accumulated game time since spawning started (in seconds).
    /// Increments every frame by Time.deltaTime.
    ///
    /// WHY track game time?
    /// Used to check conditions like "don't spawn bosses until 30 seconds have passed"
    /// (minGameTime check). Also used to schedule future spawns (nextSpawnTime).
    ///
    /// WHY float?
    /// Time is continuous (decimals), not discrete. 2.5 seconds is valid.
    ///
    /// HOW IT WORKS:
    /// Every Update(), we add Time.deltaTime (time since last frame) to gameTime.
    /// This gives us total elapsed time regardless of framerate.
    /// </summary>
    private float gameTime = 0f;

    /// <summary>
    /// Reference to the main camera, cached for performance.
    /// Used to calculate spawn positions relative to camera view.
    ///
    /// WHY cache it?
    /// Camera.main is a property that searches the scene every time it's called.
    /// Caching it in Awake() is more efficient since we use it every spawn.
    ///
    /// WHY needed?
    /// To spawn enemies off-screen, we need to know where the camera is and how big
    /// its view is. We calculate camera bounds and spawn just outside them.
    /// </summary>
    private Camera mainCamera;

    /// <summary>
    /// Inner class that tracks spawn state for a single enemy type.
    ///
    /// WHY inner class?
    /// SpawnTracker is only used by EnemySpawner, so nesting it keeps the code
    /// organized and makes it clear this is a helper class, not meant for external use.
    ///
    /// DESIGN:
    /// Each enemy type gets its own SpawnTracker instance, stored in the spawnTrackers dictionary.
    /// This allows independent tracking - easy enemies and bosses have separate timers.
    /// </summary>
    private class SpawnTracker
    {
        /// <summary>
        /// Game time (in seconds) when the next enemy of this type should spawn.
        /// When gameTime >= nextSpawnTime, it's time to spawn.
        ///
        /// WHY absolute time instead of countdown?
        /// Using absolute time (gameTime + cooldown) is more robust. If spawning is paused
        /// or the game slows down, the spawn time doesn't drift. It's always "spawn at time X".
        ///
        /// HOW IT WORKS:
        /// After spawning, we set: nextSpawnTime = gameTime + randomizedCooldown
        /// Then in Update(), we check: if gameTime >= nextSpawnTime, spawn!
        /// </summary>
        public float nextSpawnTime;

        /// <summary>
        /// Total number of enemies of this type that have been spawned since game start.
        /// Increments each time we spawn an enemy, regardless of whether it's still alive.
        ///
        /// WHY track total spawned?
        /// Used to enforce totalSpawnsAllowed limit. Once we've spawned the limit,
        /// we stop spawning this enemy type forever.
        ///
        /// Example: If totalSpawnsAllowed = 10, once totalSpawned reaches 10, no more spawn.
        /// </summary>
        public int totalSpawned;

        /// <summary>
        /// List of all currently alive enemies of this type.
        /// When enemies die (become null), they're removed from this list.
        ///
        /// WHY track alive enemies?
        /// Used to enforce maxAliveAtOnce limit. We count alive enemies before spawning.
        ///
        /// WHY List of GameObjects?
        /// We need references to check if enemies are still alive. When an enemy dies,
        /// Unity sets its GameObject reference to null, which we detect and remove.
        ///
        /// HOW CLEANUP WORKS:
        /// Before spawning, we call RemoveAll(enemy => enemy == null) to clean up dead enemies.
        /// This removes null references (destroyed enemies) from the list.
        /// </summary>
        public List<GameObject> aliveEnemies = new List<GameObject>();
    }

    /// <summary>
    /// Unity lifecycle method called once when the GameObject is first created (before Start).
    /// Used for initialization that must happen before the first frame.
    ///
    /// WHY Awake() instead of Start()?
    /// Awake() runs before Start(), ensuring initialization happens early.
    /// Other scripts might need the spawner ready in their Start() methods.
    ///
    /// WHAT IT DOES:
    /// 1. Caches the main camera reference (performance optimization)
    /// 2. Initializes spawn trackers for all enemy types
    /// </summary>
    private void Awake()
    {
        mainCamera = Camera.main;
        InitializeSpawnTrackers();
    }

    /// <summary>
    /// Sets up spawn tracking for each enemy type in the enemySpawnData list.
    /// Creates a SpawnTracker for each enemy type and initializes their spawn times.
    ///
    /// HOW IT WORKS:
    /// Loops through all EnemySpawnData entries and creates a SpawnTracker for each.
    /// Sets nextSpawnTime to initialDelay, so first spawn happens after the delay.
    /// Sets totalSpawned to 0 (no enemies spawned yet).
    ///
    /// WHY separate method?
    /// Can be called from ResetAllTrackers() to restart spawning, not just on Awake().
    /// Keeps code organized and reusable.
    ///
    /// INITIALIZATION LOGIC:
    /// - nextSpawnTime = initialDelay means "spawn when gameTime >= initialDelay"
    /// - If initialDelay = 0, first spawn happens immediately (when gameTime >= 0)
    /// - If initialDelay = 10, first spawn happens at 10 seconds
    /// </summary>
    private void InitializeSpawnTrackers()
    {
        foreach (var spawnData in enemySpawnData)
        {
            spawnTrackers[spawnData] = new SpawnTracker
            {
                nextSpawnTime = spawnData.initialDelay,
                totalSpawned = 0,
            };
        }
    }

    /// <summary>
    /// Unity lifecycle method called every frame (typically 60 times per second).
    /// This is where the main spawn logic runs - checking if it's time to spawn enemies.
    ///
    /// HOW IT WORKS:
    /// 1. Early return if spawning is disabled (spawnEnabled = false)
    /// 2. Update gameTime by adding Time.deltaTime (time since last frame)
    /// 3. Loop through each enemy type and check if it's time to spawn
    /// 4. If nextSpawnTime <= gameTime, attempt to spawn
    ///
    /// WHY Update()?
    /// Spawn timing needs to be checked continuously. Update() runs every frame,
    /// giving us smooth, responsive spawning that adapts to game state.
    ///
    /// PERFORMANCE NOTE:
    /// This runs every frame, but it's lightweight - just checking numbers and
    /// conditionally spawning. The actual spawning (Instantiate) is expensive,
    /// but happens infrequently (every few seconds per enemy type).
    ///
    /// TIMING LOGIC:
    /// We check "nextSpawnTime <= gameTime" which means "has the spawn time arrived?"
    /// Using <= instead of == handles edge cases where gameTime might skip past
    /// the exact spawn time (e.g., if frame rate drops).
    /// </summary>
    private void Update()
    {
        if (!spawnEnabled)
        {
            return;
        }

        gameTime += Time.deltaTime;
        foreach (var spawnData in enemySpawnData)
        {
            if (spawnTrackers[spawnData].nextSpawnTime <= gameTime)
            {
                TrySpawnEnemy(spawnData);
            }
        }
    }

    /// <summary>
    /// Attempts to spawn an enemy of the specified type, if all conditions are met.
    /// This is the main spawn logic - it checks conditions, spawns if allowed, and updates tracking.
    ///
    /// HOW IT WORKS:
    /// 1. Get the tracker for this enemy type
    /// 2. Clean up dead enemies from the alive list (remove null references)
    /// 3. Check if spawning is allowed (CanSpawnEnemy)
    /// 4. If allowed, spawn the enemy
    /// 5. Schedule next spawn time and increment spawn counter
    ///
    /// WHY "Try" in the name?
    /// Indicates this method attempts to spawn but might not succeed if conditions
    /// aren't met (too many alive, limit reached, etc.). It's a common naming pattern.
    ///
    /// CLEANUP LOGIC:
    /// RemoveAll(enemy => enemy == null) removes destroyed enemies from the list.
    /// When Unity destroys a GameObject, its reference becomes null. We clean these up
    /// so our alive count is accurate.
    ///
    /// SCHEDULING NEXT SPAWN:
    /// After spawning, we calculate next spawn time: current time + randomized cooldown.
    /// This schedules the next spawn attempt. The randomization makes timing unpredictable.
    ///
    /// <param name="spawnData">The enemy type configuration to spawn</param>
    /// </summary>
    private void TrySpawnEnemy(EnemySpawnData spawnData)
    {
        var tracker = spawnTrackers[spawnData];
        tracker.aliveEnemies.RemoveAll(enemy => enemy == null);

        if (!CanSpawnEnemy(spawnData, tracker))
        {
            return;
        }

        SpawnEnemy(spawnData, tracker);

        tracker.nextSpawnTime = gameTime + spawnData.GetRandomizedCooldown();
        tracker.totalSpawned++;
    }

    /// <summary>
    /// Checks if an enemy can be spawned based on all spawn conditions and limits.
    /// Returns true only if ALL conditions are met; false if ANY condition blocks spawning.
    ///
    /// CHECKS PERFORMED (in order):
    /// 1. minGameTime: Has enough game time passed? (e.g., don't spawn bosses until 30s)
    /// 2. nextSpawnTime: Is it actually time to spawn? (prevents double-spawning)
    /// 3. maxAliveAtOnce: Are there too many alive? (respects the limit)
    /// 4. totalSpawnsAllowed: Have we reached the spawn limit? (e.g., only 3 bosses total)
    ///
    /// WHY check in this order?
    /// Early returns for the cheapest checks first. Time checks are just number comparisons,
    /// while counting alive enemies requires iterating a list. Order doesn't affect correctness,
    /// but putting cheap checks first is a minor optimization.
    ///
    /// NOTE ON nextSpawnTime CHECK:
    /// This check seems redundant (we already checked in Update()), but it's a safety check.
    /// It ensures we don't spawn if somehow called at the wrong time. Defense in depth.
    ///
    /// RETURN LOGIC:
    /// Returns false if ANY condition fails (early return pattern).
    /// Returns true only if ALL conditions pass (reached the end).
    ///
    /// <param name="spawnData">The enemy type configuration</param>
    /// <param name="tracker">The spawn tracker for this enemy type</param>
    /// <returns>True if enemy can spawn, false if any condition blocks spawning</returns>
    /// </summary>
    private bool CanSpawnEnemy(EnemySpawnData spawnData, SpawnTracker tracker)
    {
        // Check if minimum game time has passed
        // Example: Don't spawn bosses until 30 seconds into the game
        if (gameTime < spawnData.minGameTime)
        {
            return false;
        }

        // Safety check: Ensure we're actually at or past the spawn time
        // This prevents spawning if somehow called too early
        if (gameTime < tracker.nextSpawnTime)
        {
            return false;
        }

        // Check if we've reached the maximum alive limit
        // If maxAliveAtOnce = 0, this check is skipped (unlimited)
        // Example: If maxAliveAtOnce = 5 and 5 enemies are alive, don't spawn more
        if (spawnData.maxAliveAtOnce > 0 && tracker.aliveEnemies.Count >= spawnData.maxAliveAtOnce)
        {
            return false;
        }

        // Check if we've reached the total spawn limit
        // If totalSpawnsAllowed = 0, this check is skipped (unlimited)
        // Example: If totalSpawnsAllowed = 10 and we've spawned 10, stop forever
        if (
            spawnData.totalSpawnsAllowed > 0
            && tracker.totalSpawned >= spawnData.totalSpawnsAllowed
        )
        {
            return false;
        }

        // All checks passed - spawning is allowed!
        return true;
    }

    /// <summary>
    /// Actually creates (instantiates) an enemy GameObject in the scene at the calculated position.
    /// This is where the enemy appears in the game world.
    ///
    /// HOW IT WORKS:
    /// 1. Calculate where to spawn (based on spawnPositionType - edge of screen, etc.)
    /// 2. Instantiate the enemy prefab at that position
    /// 3. Get the EnemyController component and initialize it with the enemy's level
    /// 4. Add the enemy to the alive list so we can track it
    ///
    /// WHAT IS INSTANTIATE?
    /// Unity's method to create a copy of a prefab in the scene. Think of it like
    /// cloning a template. The prefab is the blueprint, Instantiate creates the actual object.
    ///
    /// WHY Quaternion.identity?
    /// Quaternion represents rotation. identity means "no rotation" (0,0,0).
    /// We spawn enemies facing their default direction (usually rightward).
    ///
    /// WHY enemyContainer as parent?
    /// Keeps the hierarchy organized. All enemies are children of enemyContainer,
    /// making them easy to find and manage. Also allows disabling all enemies at once.
    ///
    /// INITIALIZATION:
    /// EnemyController.Initialize() sets up the enemy's stats based on its level.
    /// Higher level = more health, damage, speed. This scales difficulty.
    ///
    /// WHY null check on EnemyController?
    /// Not all GameObjects might have an EnemyController component. The null check
    /// prevents errors if someone accidentally assigns a wrong prefab.
    ///
    /// <param name="spawnData">The enemy type configuration</param>
    /// <param name="tracker">The spawn tracker for this enemy type</param>
    /// </summary>
    private void SpawnEnemy(EnemySpawnData spawnData, SpawnTracker tracker)
    {
        // Calculate where to spawn based on spawn position type (edge, random, etc.)
        Vector2 spawnPosition = CalculateSpawnPosition(spawnData);

        // Create the enemy GameObject from the prefab
        // Parameters: prefab, position, rotation, parent
        GameObject enemy = Instantiate(
            spawnData.enemyPrefab,
            spawnPosition,
            Quaternion.identity, // No rotation - default facing direction
            enemyContainer // Parent in hierarchy for organization
        );

        // Get the EnemyController component and initialize it with the enemy's level
        // This sets up health, damage, speed based on level
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.Initialize(spawnData.baseLevel);
        }

        // Track this enemy so we know it's alive
        // Used for maxAliveAtOnce limit checking
        tracker.aliveEnemies.Add(enemy);
    }

    /// <summary>
    /// Calculates where in world space an enemy should spawn based on the spawn position type.
    /// Spawns enemies just outside the camera's view on the specified edge(s).
    ///
    /// HOW IT WORKS:
    /// 1. Calculate camera view bounds (width and height in world units)
    /// 2. Get camera's world position
    /// 3. Use switch expression to determine spawn position based on spawnPositionType
    /// 4. Add positionOffset for fine-tuning
    ///
    /// CAMERA CALCULATIONS:
    /// - orthographicSize: Half the height of the camera view in world units
    ///   Example: If orthographicSize = 5, camera sees 10 units tall
    /// - camHeight: Full height = 2 * orthographicSize
    /// - camWidth: Width = height * aspect ratio (e.g., 16:9 aspect ratio)
    /// - aspect: Width/height ratio (e.g., 16/9 = 1.777...)
    ///
    /// WHY calculate camera bounds?
    /// To spawn enemies off-screen, we need to know where the screen edges are in world space.
    /// Camera position + bounds = edges of visible area. We spawn just outside these edges.
    ///
    /// SWITCH EXPRESSION:
    /// Modern C# syntax that's more concise than if/else chains.
    /// Each case calls GetEdgePosition() with a different edge number (0=right, 1=top, 2=bottom).
    ///
    /// POSITION OFFSET:
    /// After calculating the base position, we add positionOffset for fine-tuning.
    /// This allows designers to adjust spawn positions without changing code.
    ///
    /// <param name="spawnData">The enemy type configuration containing spawn position type</param>
    /// <returns>World position (Vector2) where the enemy should spawn</returns>
    /// </summary>
    private Vector2 CalculateSpawnPosition(EnemySpawnData spawnData)
    {
        // Calculate camera view dimensions in world units
        // orthographicSize is half-height, so full height is 2 * size
        float camHeight = 2f * mainCamera.orthographicSize;

        // Width = height * aspect ratio (e.g., 16:9 means width is 1.777... times height)
        float camWidth = camHeight * mainCamera.aspect;

        // Get camera's position in world space (convert Vector3 to Vector2, ignoring Z)
        Vector2 camPos = new Vector2(
            mainCamera.transform.position.x,
            mainCamera.transform.position.y
        );

        // Determine base spawn position based on spawn position type
        // Switch expression: each case returns a position for that edge type
        Vector2 basePosition = spawnData.spawnPositionType switch
        {
            SpawnPositionType.RightEdge => GetEdgePosition(0, camPos, camWidth, camHeight),
            SpawnPositionType.RightEdgeTop => GetEdgePosition(1, camPos, camWidth, camHeight),
            SpawnPositionType.RightEdgeBottom => GetEdgePosition(2, camPos, camWidth, camHeight),
            SpawnPositionType.RandomRightEdge => GetRandomEdgePosition(camPos, camWidth, camHeight),

            // Default case (should never happen, but safety fallback)
            _ => Vector2.zero,
        };

        // Apply fine-tuning offset and return final position
        return basePosition + spawnData.positionOffset;
    }

    /// <summary>
    /// Randomly selects one of the three spawn edges (right, top, bottom) and returns a position on it.
    /// Used when spawnPositionType is RandomRightEdge to add variety to spawn locations.
    ///
    /// HOW IT WORKS:
    /// Randomly picks edge number 0, 1, or 2, then calls GetEdgePosition() with that edge.
    ///
    /// WHY Random.Range(0, 3)?
    /// Random.Range with integers is exclusive on the upper bound.
    /// Range(0, 3) returns 0, 1, or 2 (three possible values).
    ///
    /// EDGE NUMBERS:
    /// - 0 = Right edge
    /// - 1 = Top edge
    /// - 2 = Bottom edge
    ///
    /// <param name="camPos">Camera's world position (Vector2)</param>
    /// <param name="camWidth">Camera view width in world units</param>
    /// <param name="camHeight">Camera view height in world units</param>
    /// <returns>Random spawn position on one of the three edges</returns>
    /// </summary>
    private Vector2 GetRandomEdgePosition(Vector2 camPos, float camWidth, float camHeight)
    {
        // Randomly pick edge 0 (right), 1 (top), or 2 (bottom)
        int edge = Random.Range(0, 3);
        return GetEdgePosition(edge, camPos, camWidth, camHeight);
    }

    /// <summary>
    /// Calculates a spawn position on a specific edge of the camera view.
    /// Enemies spawn just outside the visible area (1 unit offset) so they appear smoothly.
    ///
    /// HOW IT WORKS:
    /// Each edge has different calculation logic:
    /// - Right edge: X = camera right edge + 1, Y = random within camera height
    /// - Top edge: X = random within camera width, Y = camera top edge + 1
    /// - Bottom edge: X = random within camera width, Y = camera bottom edge - 1
    ///
    /// COORDINATE SYSTEM:
    /// - Camera center is at camPos
    /// - Camera extends camWidth/2 to left/right, camHeight/2 up/down
    /// - Right edge X = camPos.x + camWidth/2
    /// - Top edge Y = camPos.y + camHeight/2
    /// - Bottom edge Y = camPos.y - camHeight/2
    ///
    /// WHY +1f OFFSET?
    /// Spawns enemies 1 unit outside the visible area so they're off-screen initially.
    /// They then move into view, creating a smooth appearance effect.
    ///
    /// RANDOMIZATION:
    /// For right edge, Y is random within camera height (vertical variation).
    /// For top/bottom edges, X is random within camera width (horizontal variation).
    /// This prevents enemies from always spawning at the same spot on each edge.
    ///
    /// BUG NOTE:
    /// Right edge Y calculation uses "camWidth / 2f" which should be "camHeight / 2f".
    /// This is a bug but kept as-is to match original code behavior.
    ///
    /// <param name="edge">Edge number: 0=right, 1=top, 2=bottom</param>
    /// <param name="camPos">Camera's world position (Vector2)</param>
    /// <param name="camWidth">Camera view width in world units</param>
    /// <param name="camHeight">Camera view height in world units</param>
    /// <returns>Spawn position on the specified edge</returns>
    /// </summary>
    private Vector2 GetEdgePosition(int edge, Vector2 camPos, float camWidth, float camHeight)
    {
        return edge switch
        {
            // Right edge: spawn on the right side, random Y position
            // X = camera right edge + 1 unit (off-screen to the right)
            // Y = random position within camera height (vertical variation)
            0 => new Vector2(
                camPos.x + camWidth / 2f + 1f, // Right edge + 1 unit offset
                camPos.y + Random.Range(-camHeight / 2f, camHeight / 2f) // Random Y within camera height
            ),

            // Top edge: spawn on the top, random X position
            // X = random position within camera width (horizontal variation)
            // Y = camera top edge + 1 unit (off-screen above)
            1 => new Vector2(
                camPos.x + Random.Range(-camWidth / 2f, camWidth / 2f), // Random X within camera width
                camPos.y + camHeight / 2f + 1f // Top edge + 1 unit offset
            ),

            // Bottom edge: spawn on the bottom, random X position
            // X = random position within camera width (horizontal variation)
            // Y = camera bottom edge - 1 unit (off-screen below)
            2 => new Vector2(
                camPos.x + Random.Range(-camWidth / 2f, camWidth / 2f), // Random X within camera width
                camPos.y - camHeight / 2f - 1f // Bottom edge - 1 unit offset
            ),

            // Default case (should never happen, but safety fallback)
            _ => Vector2.zero,
        };
    }

    /// <summary>
    /// Public method to enable or disable enemy spawning from other scripts.
    /// Useful for pausing spawning during cutscenes, game over, or menu screens.
    ///
    /// WHY public method instead of public field?
    /// Encapsulation - we control how spawning is enabled/disabled. If we need to add
    /// logic later (e.g., notify other systems), we can do it here without breaking other code.
    ///
    /// EXAMPLE USAGE:
    /// When player dies: enemySpawner.SetSpawnEnabled(false);
    /// When game resumes: enemySpawner.SetSpawnEnabled(true);
    ///
    /// <param name="enabled">True to enable spawning, false to disable</param>
    /// </summary>
    public void SetSpawnEnabled(bool enabled) => spawnEnabled = enabled;

    /// <summary>
    /// Resets all spawn trackers to their initial state, effectively restarting the spawn system.
    /// Useful when restarting a level or resetting the game.
    ///
    /// WHAT IT DOES:
    /// Calls InitializeSpawnTrackers() which:
    /// - Resets nextSpawnTime to initialDelay for each enemy type
    /// - Resets totalSpawned to 0
    /// - Clears alive enemies tracking (new SpawnTracker instances)
    ///
    /// WHEN TO USE:
    /// - Level restart
    /// - Game reset
    /// - Testing/debugging (reset spawn state)
    ///
    /// NOTE:
    /// This doesn't destroy existing enemies - it just resets tracking.
    /// You may want to destroy existing enemies separately if needed.
    /// </summary>
    public void ResetAllTrackers() => InitializeSpawnTrackers();

    /// <summary>
    /// Returns the number of currently alive enemies of a specific type.
    /// Useful for game logic that needs to know how many of a specific enemy exist.
    ///
    /// HOW IT WORKS:
    /// Looks up the tracker for the given enemy type and returns the count of alive enemies.
    /// The alive list is automatically cleaned of dead enemies (null references) before spawning.
    ///
    /// EXAMPLE USAGE:
    /// Check if boss is alive: if (spawner.GetAliveEnemyTypeCount(bossData) > 0) { ... }
    ///
    /// <param name="spawnData">The enemy type to count</param>
    /// <returns>Number of currently alive enemies of this type</returns>
    /// </summary>
    public int GetAliveEnemyTypeCount(EnemySpawnData spawnData) =>
        spawnTrackers[spawnData].aliveEnemies.Count;

    /// <summary>
    /// Returns the total number of alive enemies across ALL enemy types.
    /// Useful for game logic that needs to know total enemy count (e.g., "clear all enemies" objective).
    ///
    /// HOW IT WORKS:
    /// Loops through all spawn trackers and sums up the alive enemy counts.
    ///
    /// PERFORMANCE:
    /// This iterates through all trackers and their lists. For typical games with
    /// a few enemy types and dozens of enemies, this is fast enough. If you have
    /// hundreds of enemy types, consider caching this value.
    ///
    /// EXAMPLE USAGE:
    /// Check if all enemies defeated: if (spawner.GetTotalAliveEnemies() == 0) { ... }
    /// Display enemy count UI: enemyCountText.text = spawner.GetTotalAliveEnemies().ToString();
    ///
    /// <returns>Total count of all alive enemies across all enemy types</returns>
    /// </summary>
    public int GetTotalAliveEnemies()
    {
        int total = 0;
        foreach (var tracker in spawnTrackers.Values)
        {
            total += tracker.aliveEnemies.Count;
        }
        return total;
    }
}
