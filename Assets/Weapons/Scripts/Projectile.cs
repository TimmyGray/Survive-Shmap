using UnityEngine;

namespace Weapons
{
    public abstract class Projectile : MonoBehaviour
    {
        protected Rigidbody2D _rb;

        private int damage;
        private GameObject owner;
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Initialize(int damage, GameObject owner)
        {
            this.damage = damage;
            this.owner = owner;
        }

        /// <summary>
        /// Launch the projectile with the given speed and angle.
        /// </summary>
        /// <param name="projectileSpeed">The speed of the projectile.</param>
        /// <param name="angle">The angle of the projectile will be launched at.</param>
        public abstract void Launch(int projectileSpeed, float? angle = null);

        /// <summary>
        /// Destroy the projectile when it is no longer visible.
        /// </summary>
        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// Destroy the projectile when it collides with an enemy.
        /// </summary>
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"Projectile collided with {other.name}");
            Debug.Log($"Projectile owner: {owner.name}");
            if (owner.transform.root == other.transform.root)
            {
                return;
            }

            if (other.TryGetComponent(out Health health))
            {
                health.TakeDamage(damage);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log($"Projectile collided with {other.name}, but it does not have a Health component.");
            }
        }
    }
}
