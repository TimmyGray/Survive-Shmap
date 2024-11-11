using UnityEngine;

[CreateAssetMenu(fileName = "PassiveImprovment", menuName = "Scriptable Objects/PassiveImprovment")]
public class PassiveImprovment : ScriptableObject
{
    public string passiveImprovmentName;

    public string description;

    public int levelRequired = 1;
    public int level = 1;

    public PASSIVE_IMPROVMENT_TYPE improvmentType;

    public int duration;
    public float radius;
    public float hpGain;
    public float hpRegen;


    public float timeToActivate=0.3f;

    /// <summary>
    /// Increase the characteristics of the current passive improvement according to its type.
    /// </summary>
    public void LevelUp()
    {
        Debug.Log("Passive improvment levelUp!");

        switch (improvmentType) 
        { 
            case PASSIVE_IMPROVMENT_TYPE.SHIELD:
                {
                    Debug.Log("Shield Up!");
                    break;
                }
            case PASSIVE_IMPROVMENT_TYPE.HP_REGEN:
                {
                    Debug.Log("Hp regen Up!");
                    break;
                }
            case PASSIVE_IMPROVMENT_TYPE.MAX_HP:
                {
                    Debug.Log("Max hp Up!");
                    break;
                }
        }
        level++;
    }
}
