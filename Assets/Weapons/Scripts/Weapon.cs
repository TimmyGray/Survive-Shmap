using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;

    public string description;

    public int levelRequired = 1;
    public int level = 1;
    public WEAPON_TYPE type = WEAPON_TYPE.DEFAULT;

    public float minDmg;
    public float maxDmg;

    public int numberOfProjectiles = 1;
    public int projectileSpeed = 1;
    public float attackCoolDown = 1f;
    public float dmgRadius = 0f;
    
    public bool isFlyingThrough = false;

    public Sprite weaponView;
}
