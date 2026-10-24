using UnityEngine;

namespace Weapons
{
    public class BulletCannonController : WeaponController
    {
        protected override void LaunchProjectile(int damage, GameObject owner, float baseAngle)
        {
            GameObject newProjectile_1 = Instantiate(projectile, transform.position, Quaternion.identity);

            BulletCannonProjectile bulletCannonProjectile_1 = newProjectile_1.GetComponent<BulletCannonProjectile>();

            bulletCannonProjectile_1.Initialize(damage, owner);

            bulletCannonProjectile_1.Launch(weapon.projectileSpeed, baseAngle);
        }
    }
}
