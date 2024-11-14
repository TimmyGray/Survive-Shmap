using UnityEngine;

public abstract class PassiveImrovmentController : MonoBehaviour
{
    public PassiveImprovment passiveImprovment;
    protected Animator animator;
    public abstract void Activate();
    public abstract void Deactivate();
}
