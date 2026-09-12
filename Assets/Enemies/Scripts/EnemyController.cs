using UnityEngine;
using Weapons;

public abstract class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField]
    protected Enemy enemy;
    [SerializeField]
    protected Health health;
    protected int level = 1;
    protected Rigidbody2D myRigidbody2D;

    internal virtual void Initialize(EnemyInitializeSettings initializeSettings)
    {
        level = initializeSettings.level ?? level;

        myRigidbody2D = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        health.Initialize(enemy.MaxHealth(level));

        health.OnTakeDamage.AddListener(HandleDamage);
        health.OnHeal.AddListener(HandleHeal);
        health.OnDeath.AddListener(HandleDeath);

        Debug.Log($"Enemy initialized with level {level} and health {health.CurrentHealth}");
    }

    /// <summary>
    /// Move the enemy. Direction and logic to be defined in derived classes.
    /// </summary>
    public abstract void Move();

    /// <summary>
    /// Fire the enemy's weapon(s). Logic to be defined in derived classes.
    /// </summary>
    public abstract void Fire();

    public virtual void HandleDamage(int damage, int maxHealth)
    {
        Debug.Log($"Enemy took {damage} damage. Current health: {health.CurrentHealth}/{health.MaxHealth}");
    }

    public virtual void HandleHeal(int heal, int maxHealth)
    {
        Debug.Log($"Enemy healed for {heal} health. Current health: {health.CurrentHealth}/{health.MaxHealth}");
    }

    public virtual void HandleDeath()
    {
        Debug.Log($"Enemy has died. Current health: {health.CurrentHealth}/{health.MaxHealth}");
    }
}
