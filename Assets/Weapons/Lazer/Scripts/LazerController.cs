using UnityEngine;

namespace Weapons
{
    public class LazerController : WeaponController
    {
        protected override void LaunchProjectile(float damage, GameObject owner)
        {
            GameObject newProjectile_1 = Instantiate(projectile, transform.position, Quaternion.identity);
            GameObject newProjectile_2 = Instantiate(projectile, transform.position, Quaternion.identity);
            GameObject newProjectile_3 = Instantiate(projectile, transform.position, Quaternion.identity);

            LazerProjectile lazerProjectile_1 = newProjectile_1.GetComponent<LazerProjectile>();
            LazerProjectile lazerProjectile_2 = newProjectile_2.GetComponent<LazerProjectile>();
            LazerProjectile lazerProjectile_3 = newProjectile_3.GetComponent<LazerProjectile>();

            lazerProjectile_1.Initialize(damage, owner);
            lazerProjectile_2.Initialize(damage, owner);
            lazerProjectile_3.Initialize(damage, owner);

            lazerProjectile_1.Launch(weapon.projectileSpeed, 10);
            lazerProjectile_2.Launch(weapon.projectileSpeed, 0);
            lazerProjectile_3.Launch(weapon.projectileSpeed, -10);
        }
    }
}
