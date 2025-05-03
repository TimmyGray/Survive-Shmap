using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class Enemy : ScriptableObject
{
    public string Name = "Enemy";

    public float currentHealth = 100f;
    public float maxHealth = 100f;

    public float minDmg = 1f;
    public float maxDmg = 2f;

    public float speed = 10f;

    public int level = 1;

    public List<GameObject> currentWeapons = new List<GameObject>();
    public List<GameObject> currentPassiveImprovments = new List<GameObject>();

    public List<Perk> perks = new List<Perk>(); 
}
