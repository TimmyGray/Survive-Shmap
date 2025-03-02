using UnityEngine;

public abstract class PerkController : MonoBehaviour    
{
    public Perk perk;
    /// <summary>
    /// Apply the perk to the player
    /// </summary>
    public abstract void ApplyPerk(Character character);

    /// <summary>
    /// Remove the perk from the player
    /// </summary>
    public abstract void RemovePerk(Character character);

    /// <summary>
    /// Level up the parameters of the perk
    /// </summary>
    protected abstract void LevelUpParameters();

    /// <summary>
    /// Upgrade a parameter by a percentage
    /// </summary>
    /// <param name="upgradeParameter">The parameter to upgrade</param>
    /// <param name="character">The character to upgrade</param>
    private void PercentageChange(ParameterToUpgrade upgradeParameter, Character character, CHANGE_DIRECTION direction)
    {
        var fieldInfo = character.GetType().GetField(upgradeParameter.name);
        var propertyInfo = character.GetType().GetProperty(upgradeParameter.name);

        if (fieldInfo != null)
        {
            float currentValue = (float)fieldInfo.GetValue(character);
            float percentageIncrease = float.Parse(upgradeParameter.value) / 100f;
            
            fieldInfo.SetValue(character, currentValue * (direction == CHANGE_DIRECTION.INCREASE ? 1 + percentageIncrease : 1 - percentageIncrease));
        }
        else if (propertyInfo != null)
        {
            float currentValue = (float)propertyInfo.GetValue(character);
            float percentageIncrease = float.Parse(upgradeParameter.value) / 100f;
            propertyInfo.SetValue(character, currentValue * (direction == CHANGE_DIRECTION.INCREASE ? 1 + percentageIncrease : 1 - percentageIncrease));
        }
        else
        {
            Debug.LogError($"Field or property '{upgradeParameter.name}' not found on Character.");
        }
    }

    /// <summary>
    /// Upgrade a parameter according to the type
    /// </summary>
    /// <param name="upgradeParameter">The parameter to upgrade</param>
    /// <param name="character">The character whom the perk is applied</param>
    protected void ChangeParameter(ParameterToUpgrade upgradeParameter, Character character, CHANGE_DIRECTION direction)
    {
        switch (upgradeParameter.type)
        {
            case CHANGE_TYPE.PERCENTAGE:
                PercentageChange(upgradeParameter, character, direction);
                break;
        }
    }

    /// <summary>
    /// Level up the perk
    /// </summary>
    /// <param name="character">To correct upgrade character parameters after level up its required to remove perk before level up and apply it after level up</param>
    public void LevelUp(Character character)
    {
        perk.level++;
        RemovePerk(character);
        LevelUpParameters();
        ApplyPerk(character);
    }
}

