using UnityEngine;

namespace Weapons
{
    public class PlasmaProjectile : Projectile
    {
        public override void Launch(int speed, float? angle = null)
        {
            if (angle != null)
            {
                transform.eulerAngles = new Vector3(0, 0, angle.Value);
            }

            _rb.AddForce((Vector2)transform.right * speed, ForceMode2D.Impulse);
        }
    }
}
