using UnityEngine;

namespace PassiveImprovments
{
    public abstract class PassiveImprovmentController : MonoBehaviour
    {
        public PassiveImprovment passiveImprovment;
        protected Animator animator;
        protected bool isActive;
        protected bool inUse;

        /// <summary>
        /// Activate the passive improvment.
        /// </summary>
        public abstract void Activate();

        /// <summary>
        /// Deactivate the passive improvment.
        /// </summary>
        /// <param name="fullDeactivate">If true, the passive improvment will be fully deactivated and removed from the player.</param>
        public abstract void Deactivate(bool? fullDeactivate = false);

        /// <summary>
        /// Level up the passive improvment.
        /// </summary>
        public abstract void LevelUp();
    }
}
