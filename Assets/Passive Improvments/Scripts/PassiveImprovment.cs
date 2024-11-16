using UnityEngine;

namespace PassiveImprovments
{
    [CreateAssetMenu(fileName = "PassiveImprovment", menuName = "Scriptable Objects/PassiveImprovment")]
    public class PassiveImprovment : ScriptableObject
    {
        public string passiveImprovmentName;

        public string description;

        public int levelRequired = 1;
        public int level = 1;

        public PASSIVE_IMPROVMENT_TYPE type;

        public int duration;
        public float radius;
        public float hpGain;
        public float hpRegen;
        public float cooldown;

        public float activatingTime=0.3f;
    }
}
