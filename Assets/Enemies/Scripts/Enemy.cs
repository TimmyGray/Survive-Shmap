using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class Enemy : ScriptableObject
{
    [Header("Base stats")]
    public string Name = "Enemy";
    public float maxHealth = 100f;

    public float minDmg = 1f;
    public float maxDmg = 2f;

    public float speed = 10f;

    public float spawnCooldown = 5f;

    [Tooltip("Stat percentage increase per level")]
    public float maxHealthIncreasePerLevel = 0.15f;
    public float minDmgIncreasePerLevel = 0.1f;
    public float maxDmgIncreasePerLevel = 0.1f;
    public float speedIncreasePerLevel = 0.1f;

    public float MaxHealth(int level) => CalculateStat(maxHealth, maxHealthIncreasePerLevel, level);

    public float MinDmg(int level) => CalculateStat(minDmg, minDmgIncreasePerLevel, level);

    public float MaxDmg(int level) => CalculateStat(maxDmg, maxDmgIncreasePerLevel, level);

    public float Speed(int level) => CalculateStat(speed, speedIncreasePerLevel, level);

    private float CalculateStat(float baseStat, float increasePerLevel, int level)
    {
        return baseStat * (1 + increasePerLevel * level);
    }

    public List<GameObject> currentWeapons = new List<GameObject>();
    public List<GameObject> currentPassiveImprovments = new List<GameObject>();

    public List<Perk> perks = new List<Perk>();
}
