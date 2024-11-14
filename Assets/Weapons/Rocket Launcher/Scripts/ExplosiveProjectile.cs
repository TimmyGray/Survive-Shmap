using UnityEngine;
using System.Collections;

namespace Weapons
{
    public class ExplosiveProjectile: Projectile
    {
        public float delayBeforeAutonavigation = 0.2f;
        
        public override void Launch(float projectileSpeed, float? angle = null)
        {
            if (angle !=null)
            {
                transform.eulerAngles = new Vector3(0, 0, angle.Value);
            }

            Vector2 direction = transform.right;
            _rb.AddForce(direction * projectileSpeed, ForceMode2D.Impulse);

            GameObject closestTarget = FindClosestTarget();
            if (closestTarget != null)
            {
                StartCoroutine(AutoNavigateToTarget(closestTarget, projectileSpeed));
            }
        }

        /// <summary>
        /// Find the closest target to the projectile.
        /// All Enemies should have the tag "Enemy" to be detected.
        /// </summary>
        private GameObject FindClosestTarget(){
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");
            if (targets.Length == 0)
            {
                return null;
            }

            GameObject closestTarget = null;
            float closestDistance = Mathf.Infinity;

            foreach (GameObject target in targets)
            {
                float distance = Vector2.Distance(transform.position, target.transform.position);
                if (distance < closestDistance){
                    closestTarget = target;
                }
            }

            return closestTarget;
        }

        /// <summary>
        /// Auto navigate the projectile to the target after a short delay for the nice effect.
        /// This function should be called inside a coroutine.
        /// </summary>
        private IEnumerator AutoNavigateToTarget(GameObject target, float projectileSpeed){
            yield return new WaitForSeconds(delayBeforeAutonavigation);
            
            while (target != null)
            {
                Vector2 direction = (target.transform.position - transform.position).normalized;
                _rb.linearVelocity = direction * projectileSpeed;
                
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
                
                yield return null;
            }
        }
    }
}

