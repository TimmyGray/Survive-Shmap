using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class Enemy : ScriptableObject
{
    [Header("Base stats")]
    public string Name = "Enemy";
    public int maxHealth = 100;

    public int minDmg = 1;
    public int maxDmg = 2;

    public int speed = 10;

    [Tooltip("Stat percentage increase per level")]
    public float maxHealthIncreasePerLevel = 0.15f;
    public float minDmgIncreasePerLevel = 0.1f;
    public float maxDmgIncreasePerLevel = 0.1f;
    public float speedIncreasePerLevel = 0.1f;

    public int MaxHealth(int level) => CalculateStat(maxHealth, maxHealthIncreasePerLevel, level);

    public int MinDmg(int level) => CalculateStat(minDmg, minDmgIncreasePerLevel, level);

    public int MaxDmg(int level) => CalculateStat(maxDmg, maxDmgIncreasePerLevel, level);

    public int Speed(int level) => CalculateStat(speed, speedIncreasePerLevel, level);

    private int CalculateStat(int baseStat, float increasePerLevel, int level)
    {
        return Mathf.RoundToInt(baseStat * (1 + increasePerLevel * (level - 1)));
    }

    public List<GameObject> currentWeapons = new List<GameObject>();
    public List<GameObject> currentPassiveImprovments = new List<GameObject>();

    public List<Perk> perks = new List<Perk>();
}
