using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Perk", menuName = "Scriptable Objects/Perk")]
public class Perk : ScriptableObject
{
    public string perkName;
    public string description;

    public int levelRequired = 1;
    public int level = 1;

    public List<ParameterToUpgrade> upgradeParameters = new List<ParameterToUpgrade>();

    public Sprite perkIcon; 
}
