using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
    public class Weapon : ScriptableObject
    {
        public string weaponName;

        public string description;

        public int levelRequired = 1;
        public int level = 1;
        public WEAPON_TYPE type = WEAPON_TYPE.DEFAULT;

        public int minDmg;
        public int maxDmg;

        public int numberOfProjectiles = 1;
        public int projectileSpeed = 1;
        public float attackCoolDown = 1f;
        public int dmgRadius = 0;
    
        public bool isFlyingThrough = false;

        public Sprite weaponView;
    }
}
