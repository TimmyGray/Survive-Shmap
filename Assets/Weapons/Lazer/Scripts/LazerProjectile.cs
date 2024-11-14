using UnityEngine;

namespace Weapons
{
    public class LazerProjectile : Projectile
    {
        public override void Launch(float projectileSpeed, float? angle = null)
        {
            if (angle != null)
            {
                transform.eulerAngles = new Vector3(0, 0, angle.Value);
            }

            // Convert angle to direction vector
            Vector2 direction = transform.right; // This gets the local right direction based on rotation
            _rb.AddForce(direction * projectileSpeed, ForceMode2D.Impulse);     
        }
    }
}
