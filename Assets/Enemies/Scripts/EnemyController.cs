using UnityEngine;
using Weapons;

public abstract class EnemyController : MonoBehaviour
{
    public Enemy enemyData;
    protected Rigidbody2D myRigidbody2D;

    protected virtual void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
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
        enemyData.currentHealth -= amount;
        if (enemyData.currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Handle enemy death. Logic to be defined in derived classes (destroy, animation, etc).
    /// </summary>
    protected abstract void Die();
}
