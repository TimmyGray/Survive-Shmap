using UnityEngine;

namespace Weapons{
    public class PlasmaProjectile : Projectile
    {
        public override void Launch(float speed, float? angle = null)
        {
            _rb.AddForce(new Vector2(speed,0), ForceMode2D.Impulse);
        }
    }
}
