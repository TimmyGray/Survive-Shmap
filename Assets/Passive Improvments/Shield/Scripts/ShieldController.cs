using System.Collections;
using UnityEngine;

namespace PassiveImprovments
{
    public class ShieldController : PassiveImprovmentController
    {
        private CircleCollider2D circleCollider;
        private float timeToDeactivate;
        private float timeToActivate;
        private bool transitioning;
        void Awake()
        {
            circleCollider = GetComponent<CircleCollider2D>();
            timeToDeactivate = passiveImprovment.duration;
            timeToActivate = 0;
            animator = GetComponent<Animator>();
        }

        void FixedUpdate()
        {
            if(inUse)
            {
                if(timeToActivate <= 0 && !isActive && !transitioning)
                {
                    Activate();
                } else if(timeToDeactivate > 0)
                {
                    timeToDeactivate -= Time.fixedDeltaTime;
                }

                if(timeToDeactivate <= 0 && isActive && !transitioning)
                {
                    Deactivate();
                } else if (timeToActivate > 0)
                {
                    timeToActivate -= Time.fixedDeltaTime;
                }
            }

        }
    

        public override void Activate()
        {
            transitioning = true;
            animator.SetTrigger("Activate");
            StartCoroutine(ActivatingProcess());
        }

        /// <summary>
        /// Activate the shield.
        /// Using a coroutine to animate the shield's collider growing in size.
        /// </summary>
        public IEnumerator ActivatingProcess()
        {
            while(circleCollider.radius < passiveImprovment.radius)
            {
                circleCollider.radius += passiveImprovment.activatingTime;
                yield return null;
            }
            yield return new WaitForSeconds(passiveImprovment.activatingTime);
            
            timeToDeactivate = passiveImprovment.duration;
            timeToActivate = 0;
            isActive = true;
            inUse = true;
            transitioning = false;
            animator.SetBool("IsActive", true);
        }

        public override void Deactivate(bool? fullDeactivate = false)
        {
            transitioning = true;
            if(fullDeactivate == true)
            {
                    inUse = false;
            }
            animator.SetTrigger("Deactivate");
            StartCoroutine(DeactivatingProcess());
        }

        /// <summary>
        /// Deactivate the shield.
        /// Using a coroutine to animate the shield's collider shrinking in size.
        /// </summary>
        public IEnumerator DeactivatingProcess()
        {
            while(circleCollider.radius > 0.01f)
            {
                circleCollider.radius -= passiveImprovment.activatingTime;
                yield return null;
            }
            yield return new WaitForSeconds(passiveImprovment.activatingTime);
            
            timeToActivate = passiveImprovment.cooldown;
            timeToDeactivate = 0;
            isActive = false;
            transitioning = false;
            animator.SetBool("IsActive", false);
        }

        public override void LevelUp()
        {
            
        }
    }
}
