using UnityEngine;

namespace Weapons{
    public class LazerController : WeaponController
    {
        protected override void LaunchProjectile()
        {
            GameObject newProjectile_1 = Instantiate(projectile, transform.position, Quaternion.identity);
            GameObject newProjectile_2 = Instantiate(projectile, transform.position, Quaternion.identity);
            GameObject newProjectile_3 = Instantiate(projectile, transform.position, Quaternion.identity);
            
            newProjectile_1.GetComponent<LazerProjectile>().Launch(weapon.projectileSpeed, 10);
            newProjectile_2.GetComponent<LazerProjectile>().Launch(weapon.projectileSpeed, 0);
            newProjectile_3.GetComponent<LazerProjectile>().Launch(weapon.projectileSpeed, -10);    
        }
    }
}
