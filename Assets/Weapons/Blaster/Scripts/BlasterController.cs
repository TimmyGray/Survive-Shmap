using UnityEngine;

namespace Weapons{
    public class BlasterController : WeaponController
    {
        protected override void LaunchProjectile()
        {
            GameObject newProjectile_1 = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y + 0.2f), Quaternion.identity);
            GameObject newProjectile_2 = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y - 0.2f), Quaternion.identity);
            
            newProjectile_1.GetComponent<PlasmaProjectile>().Launch(weapon.projectileSpeed);
            newProjectile_2.GetComponent<PlasmaProjectile>().Launch(weapon.projectileSpeed);
        }
    }
}

