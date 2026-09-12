using UnityEngine;

namespace Weapons
{
    public abstract class WeaponController : MonoBehaviour
    {

        public Weapon weapon;
        public GameObject projectile;
        public float timeToNextAttack = 0f;

        /// <summary>
        /// Increase the characteristics of the current weapon according its type
        /// </summary>
        public void LevelUp() { }

        /// <summary>
        /// Fire the current weapon if the cooldown allows.
        /// <param name="maxDmg">The maximum damage to add to the weapon's base damage.</param>
        /// <param name="minDmg">The minimum damage to add to the weapon's base damage.</param>
        /// <param name="owner">The owner of the projectile.</param>
        /// </summary>
        public void Fire(GameObject owner, int maxDmg = 0, int minDmg = 0)
        {
            if (timeToNextAttack <= 0)
            {
                var finalDamage = Random.Range(minDmg + weapon.minDmg, maxDmg + weapon.maxDmg + 1);
                LaunchProjectile(finalDamage, owner);
                timeToNextAttack = weapon.attackCoolDown;
            }
        }

        /// <summary>
        /// Launch the projectile of the current weapon.
        /// <param name="damage">The damage the projectile will deal.</param>
        /// <param name="owner">The owner of the projectile.</param>
        /// </summary>
        protected abstract void LaunchProjectile(int damage, GameObject owner);
    }
}
