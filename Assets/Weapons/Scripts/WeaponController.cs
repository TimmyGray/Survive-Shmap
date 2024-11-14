using UnityEngine;

namespace Weapons{
    public abstract class WeaponController : MonoBehaviour
    {  

        public Weapon weapon;
        public GameObject projectile;
        public float timeToNextAttack = 0f;

        /// <summary>
        /// Increase the characteristics of the current weapon according its type
        /// </summary>
        public void LevelUp(){}

        /// <summary>
        /// Fire the current weapon if the cooldown allows.
        /// </summary>
        public void Fire()
        {
            if (timeToNextAttack <= 0)
            {
                LaunchProjectile();
                timeToNextAttack = weapon.attackCoolDown;   
            }
        }

        /// <summary>
        /// Launch the projectile of the current weapon.
        /// </summary>
        protected abstract void LaunchProjectile();
    }
}
