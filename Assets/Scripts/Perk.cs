using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Perk", menuName = "Scriptable Objects/Perk")]
public class Perk : ScriptableObject
{
    public string perkName;
    public string description;

    public int levelRequired = 1;

    public List<Tuple<string, dynamic>> upgradeParameters = new List<Tuple<string,dynamic>>();

    public Sprite perkIcon;   
}
