using UnityEngine;

public class IncreaseAllDMG : PerkController
{
    public override void ApplyPerk(Character character)
    {
        Debug.Log("Increase All DMG");
        Debug.Log($"Character max dmg: {character.maxDmg}");
        Debug.Log($"Character min dmg: {character.minDmg}");
        Debug.Log($"Perk characteristisc: {perk.perkName}");

        try
        {
           foreach (var item in perk.upgradeParameters)
            {
                Debug.Log($"Upgrade parameter: {item.name}");
                Debug.Log($"Upgrade parameter: {item.type}");
                Debug.Log($"Upgrade parameter: {item.value}");
                ChangeParameter(item, character, CHANGE_DIRECTION.INCREASE); 
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error while increasing all dmg: " + e.Message);
        }
    }

    public override void RemovePerk(Character character)
    {
        Debug.Log("Decrease All DMG");
        Debug.Log($"Character max dmg: {character.maxDmg}");
        Debug.Log($"Character min dmg: {character.minDmg}");
        Debug.Log($"Perk characteristisc: {perk.perkName}");

        try
        {
           foreach (var item in perk.upgradeParameters)
            {
                Debug.Log($"Parameter to decrease: {item.name}");
                Debug.Log($"Parameter to decrease: {item.type}");
                Debug.Log($"Parameter to decrease: {item.value}");
                ChangeParameter(item, character, CHANGE_DIRECTION.DECREASE); 
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error while decreasing all dmg: " + e.Message);
        }
    }

    protected override void LevelUpParameters()
    {
        foreach (var item in perk.upgradeParameters)
        {
            item.value += 10;
        }
    }
}

