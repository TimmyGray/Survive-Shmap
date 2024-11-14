using UnityEngine;

namespace Weapons
{
    public abstract class Projectile : MonoBehaviour
    {
        protected Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Launch the projectile with the given speed and angle.
        /// </summary>
        /// <param name="projectileSpeed">The speed of the projectile.</param>
        /// <param name="angle">The angle of the projectile will be launched at.</param>
        public abstract void Launch(float projectileSpeed, float? angle = null);

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
            if (other.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }
        }
    }
}