using UnityEngine;

/// <summary>
/// ScriptableObject that defines how and when a specific enemy type should spawn.
/// ScriptableObjects are Unity's way of creating reusable data assets that can be shared
/// across multiple objects without duplicating data. This allows designers to create
/// different spawn configurations (e.g., "Easy Wave", "Boss Wave") without touching code.
///
/// WHY ScriptableObject?
/// - Can be created in Unity Editor without code
/// - Can be reused across multiple spawners
/// - Easy to tweak values and see changes immediately
/// - Separates data (this file) from logic (EnemySpawner.cs)
/// </summary>
[CreateAssetMenu(fileName = "EnemySpawnData", menuName = "Scriptable Objects/EnemySpawnData")]
public class EnemySpawnData : ScriptableObject
{
    /// <summary>
    /// The prefab (pre-configured GameObject template) of the enemy to spawn.
    /// Prefabs are like blueprints - when we spawn, Unity creates a copy of this prefab.
    /// </summary>
    [Header("Enemy reference")]
    public GameObject enemyPrefab;

    /// <summary>
    /// Base time (in seconds) between spawning enemies of this type.
    /// After spawning one enemy, we wait this long before spawning the next.
    /// Example: If spawnCooldown = 5f, enemies spawn every 5 seconds (plus variation).
    /// </summary>
    [Header("Spawn timings")]
    [Tooltip("Time in seconds between spawns")]
    public float spawnCooldown = 5f;

    /// <summary>
    /// Random variation added/subtracted from spawnCooldown to make spawning feel less predictable.
    /// The actual cooldown will be: spawnCooldown ± spawnCooldownVariation
    /// Example: If spawnCooldown = 5f and spawnCooldownVariation = 1f,
    /// actual cooldown will be between 4f and 6f seconds (randomly chosen each time).
    /// </summary>
    [Tooltip("Random variance added to the spawn cooldown (+-)")]
    public float spawnCooldownVariation = 1f;

    /// <summary>
    /// How long to wait (in seconds) before spawning the FIRST enemy of this type.
    /// Useful for staggering different enemy types - e.g., wait 10 seconds before spawning bosses.
    ///
    /// WHY initialDelay separate from spawnCooldown?
    /// Allows different enemy types to start spawning at different times, creating
    /// a progressive difficulty curve. Easy enemies spawn immediately, harder ones spawn later.
    ///
    /// Example: If initialDelay = 10f, the first enemy spawns 10 seconds after game starts,
    /// then subsequent enemies follow the spawnCooldown pattern.
    /// </summary>
    [Tooltip("Delay before the first spawn")]
    public float initialDelay = 0f;

    /// <summary>
    /// Maximum number of enemies of THIS type that can be alive simultaneously.
    /// Once this limit is reached, no more enemies of this type spawn until some die.
    ///
    /// WHY limit alive enemies?
    /// Prevents overwhelming the player with too many enemies at once. Also helps with
    /// performance - too many GameObjects can slow down the game.
    ///
    /// WHY 0 = unlimited?
    /// Using 0 as "unlimited" is a common pattern. It's easier than having a separate
    /// boolean flag. The spawner checks "if maxAliveAtOnce > 0" to know if there's a limit.
    ///
    /// Example: If maxAliveAtOnce = 5, once 5 enemies of this type exist, spawning stops
    /// until one dies, then a new one can spawn.
    /// </summary>
    [Header("Spawn limits")]
    [Tooltip("Maximum enemies of this type alive at once. 0 = unlimited")]
    public int maxAliveAtOnce = 0;

    /// <summary>
    /// Total number of enemies of this type that can spawn during the entire game session.
    /// Once this many enemies have been spawned (regardless of whether they're still alive),
    /// no more enemies of this type will spawn.
    ///
    /// WHY total spawn limit?
    /// Useful for special enemies that should only appear a few times (e.g., "spawn 3 bosses total").
    /// Also useful for testing - spawn exactly 10 enemies, then stop.
    ///
    /// WHY 0 = unlimited?
    /// Same pattern as maxAliveAtOnce - 0 means "no limit", any positive number is the limit.
    ///
    /// Example: If totalSpawnsAllowed = 10, after spawning 10 enemies total, this enemy
    /// type stops spawning forever, even if all 10 are dead.
    /// </summary>
    [Tooltip("Total spawns allowed. 0 = unlimited")]
    public int totalSpawnsAllowed = 0;

    /// <summary>
    /// Minimum game time (in seconds) that must pass before this enemy type can start spawning.
    /// Similar to initialDelay, but this is checked continuously - if game time drops below
    /// this (e.g., after a reset), spawning stops again.
    ///
    /// WHY separate from initialDelay?
    /// initialDelay is a one-time delay. minGameTime is a continuous gate - enemies only
    /// spawn when the game has progressed far enough. Useful for difficulty progression.
    ///
    /// Example: If minGameTime = 30f, this enemy type won't spawn until 30 seconds have
    /// passed in the game, regardless of initialDelay.
    /// </summary>
    [Header("Spawn conditions")]
    [Tooltip("Minimum game time required before spawning this enemy type")]
    public float minGameTime = 0f;

    /// <summary>
    /// The level/strength of enemies when they spawn. Higher level = stronger enemies
    /// (more health, more damage, faster speed). This is passed to EnemyController.Initialize().
    ///
    /// WHY level system?
    /// Allows enemies to scale with game progression. Early enemies are level 1, later
    /// enemies are level 5+. This creates increasing difficulty without needing different prefabs.
    ///
    /// HOW it works:
    /// The EnemyController.Initialize() method uses this level to calculate stats using
    /// formulas in the Enemy ScriptableObject (e.g., health = baseHealth * (1 + 0.15 * level)).
    ///
    /// Example: If baseLevel = 3, spawned enemies will be level 3, making them stronger
    /// than level 1 enemies.
    /// </summary>
    [Tooltip(
        "Enemy level when spawning this enemy type (can be overridden by difficulty settings)"
    )]
    public int baseLevel = 1;

    /// <summary>
    /// Where on the screen edges enemies should spawn. Options: RightEdge, TopEdge, BottomEdge, RandomEdge.
    ///
    /// WHY spawn on edges?
    /// Enemies should appear off-screen and move into view, not pop into existence in front
    /// of the player. This feels more natural and gives players time to react.
    ///
    /// WHY no LeftEdge?
    /// The game is designed for the player to shoot rightward. Spawning enemies on the left
    /// (behind the player) would be confusing and break the game flow.
    ///
    /// HOW it works:
    /// The spawner calculates camera bounds, then places enemies just outside the visible
    /// area on the specified edge(s).
    /// </summary>
    [Header("Spawn position")]
    public SpawnPositionType spawnPositionType = SpawnPositionType.RightEdge;

    /// <summary>
    /// Additional offset applied to the calculated spawn position.
    /// Useful for fine-tuning where exactly enemies appear (e.g., spawn slightly higher/lower).
    ///
    /// WHY Vector2?
    /// Represents 2D coordinates (x, y). Unity uses Vector2 for 2D positions.
    ///
    /// Example: If spawnPositionType = RightEdge and positionOffset = (0, 2),
    /// enemies spawn on the right edge but 2 units higher than normal.
    /// </summary>
    public Vector2 positionOffset = Vector2.zero;

    /// <summary>
    /// Calculates a randomized spawn cooldown by adding random variation to the base cooldown.
    /// This makes enemy spawning feel less predictable and more natural.
    ///
    /// HOW it works:
    /// Takes spawnCooldown (e.g., 5 seconds) and adds/subtracts a random amount up to
    /// spawnCooldownVariation (e.g., ±1 second), resulting in a value between 4-6 seconds.
    ///
    /// WHY Random.Range with negative and positive?
    /// Random.Range(-variation, +variation) gives us a symmetric range. If variation = 1,
    /// we get values between -1 and +1, which we add to the base cooldown.
    ///
    /// Example:
    /// - spawnCooldown = 5f, spawnCooldownVariation = 1f
    /// - Random.Range(-1f, 1f) might return 0.3f
    /// - Result: 5f + 0.3f = 5.3f seconds until next spawn
    ///
    /// <returns>A random cooldown time between (spawnCooldown - variation) and (spawnCooldown + variation)</returns>
    /// </summary>
    public float GetRandomizedCooldown()
    {
        return spawnCooldown + Random.Range(-spawnCooldownVariation, spawnCooldownVariation);
    }
}

/// <summary>
/// Defines which edge of the screen enemies should spawn on.
///
/// WHY no LeftEdge?
/// The game design assumes players shoot rightward. Spawning enemies on the left
/// (behind the player) would be confusing and break immersion. Enemies should
/// appear ahead of the player, not behind them.
/// </summary>
public enum SpawnPositionType
{
    /// <summary>
    /// Spawn enemies on the right edge of the screen (most common).
    /// Enemies appear from the right side and move left toward the player.
    /// </summary>
    RightEdge,

    /// <summary>
    /// Spawn enemies on the top edge of the screen.
    /// Enemies appear from above and move downward.
    /// </summary>
    RightEdgeTop,

    /// <summary>
    /// Spawn enemies on the bottom edge of the screen.
    /// Enemies appear from below and move upward.
    /// </summary>
    RightEdgeBottom,

    /// <summary>
    /// Randomly choose one of the three edges (Right, Top, or Bottom) for each spawn.
    /// Adds variety - enemies can come from different directions, making gameplay
    /// more dynamic and less predictable.
    /// </summary>
    RandomRightEdge,
}
