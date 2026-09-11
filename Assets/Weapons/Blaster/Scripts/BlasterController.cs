using UnityEngine;

namespace Weapons
{
    public class BlasterController : WeaponController
    {
        protected override void LaunchProjectile(float damage, GameObject owner)
        {
            GameObject newProjectile_1 = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y + 0.2f), Quaternion.identity);
            GameObject newProjectile_2 = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y - 0.2f), Quaternion.identity);

            PlasmaProjectile plasmaProjectile_1 = newProjectile_1.GetComponent<PlasmaProjectile>();
            PlasmaProjectile plasmaProjectile_2 = newProjectile_2.GetComponent<PlasmaProjectile>();

            plasmaProjectile_1.Initialize(damage, owner);
            plasmaProjectile_2.Initialize(damage, owner);

            plasmaProjectile_1.Launch(weapon.projectileSpeed);
            plasmaProjectile_2.Launch(weapon.projectileSpeed);
        }
    }
}

