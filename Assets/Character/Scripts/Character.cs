using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Objects/Character")]
public class Character : ScriptableObject
{
    public string characterName = "Player";

    public float currentHealth = 100f;
    public float maxHealth = 100f;

    public int currentExp = 0;
    public int expToNextLvl = 100;
    public int level = 1;

    public float minDmg = 1f;
    public float maxDmg = 2f;
    public float speed = 10f;

    public List<GameObject> currentWeapons = new List<GameObject>();
    public List<PassiveImprovment> currentPassiveImprovments = new List<PassiveImprovment>();

    public List<Perk> perks = new List<Perk>();

    public List<GameObject> allWeapons = new List<GameObject>();
}