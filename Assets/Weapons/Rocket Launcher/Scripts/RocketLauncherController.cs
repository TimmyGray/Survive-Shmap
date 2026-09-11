using UnityEngine;

namespace Weapons
{
    /// <summary>
    /// Allows the player to launch explosive projectiles with auto navigation.
    /// All Enemies should have the tag "Enemy" to be detected.
    /// </summary>
    public class RocketLauncherController : WeaponController
    {
        protected override void LaunchProjectile(float damage, GameObject owner)
        {
            GameObject newProjectile_1 = Instantiate(projectile, transform.position, Quaternion.identity);
            GameObject newProjectile_2 = Instantiate(projectile, transform.position, Quaternion.identity);

            ExplosiveProjectile explosiveProjectile_1 = newProjectile_1.GetComponent<ExplosiveProjectile>();
            ExplosiveProjectile explosiveProjectile_2 = newProjectile_2.GetComponent<ExplosiveProjectile>();

            explosiveProjectile_1.Initialize(damage, owner);
            explosiveProjectile_2.Initialize(damage, owner);

            explosiveProjectile_1.Launch(weapon.projectileSpeed, 20);
            explosiveProjectile_2.Launch(weapon.projectileSpeed, -20);
        }
    }
}
