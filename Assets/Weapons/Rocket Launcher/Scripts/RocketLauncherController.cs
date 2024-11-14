using UnityEngine;

namespace Weapons
{
    /// <summary>
    /// Allows the player to launch explosive projectiles with auto navigation.
    /// All Enemies should have the tag "Enemy" to be detected.
    /// </summary>
    public class RocketLauncherController : WeaponController
    {
        protected override void LaunchProjectile()
        {
            GameObject newProjectile_1 = Instantiate(projectile, transform.position, Quaternion.identity);
            GameObject newProjectile_2 = Instantiate(projectile, transform.position, Quaternion.identity);

            newProjectile_1.GetComponent<ExplosiveProjectile>().Launch(weapon.projectileSpeed, 20);
            newProjectile_2.GetComponent<ExplosiveProjectile>().Launch(weapon.projectileSpeed, -20);
        }
    }
}
