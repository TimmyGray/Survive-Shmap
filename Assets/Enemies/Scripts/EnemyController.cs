using UnityEngine;
using Weapons;

public abstract class EnemyController : MonoBehaviour
{
    [SerializeField]
    protected Enemy enemy;

    protected float currentHealth;
    protected int level = 1;
    protected Rigidbody2D myRigidbody2D;

    internal virtual void Initialize(int? level = null)
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        currentHealth = enemy.MaxHealth(level ?? this.level);
        this.level = level ?? this.level;
    }

    /// <summary>
    /// Move the enemy. Direction and logic to be defined in derived classes.
    /// </summary>
    public abstract void Move(Vector2 direction);

    /// <summary>
    /// Fire the enemy's weapon(s). Logic to be defined in derived classes.
    /// </summary>
    public abstract void Fire();

    /// <summary>
    /// Apply damage to the enemy. Handles death if health drops to zero or below.
    /// </summary>
    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Handle enemy death. Logic to be defined in derived classes (destroy, animation, etc).
    /// </summary>
    protected abstract void Die();
}
