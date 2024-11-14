using System.Collections;
using System.Security.AccessControl;
using UnityEngine;
using System.Collections.Generic;

public class ShieldController : PassiveImrovmentController
{
    private CircleCollider2D circleCollider;
    private bool isActive;

    private float timeToEnd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        timeToEnd = passiveImprovment.duration;
        isActive = false;
        gameObject.SetActive(false);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive){
            timeToEnd -= Time.deltaTime;
            if(timeToEnd <= 0)
            {
                Deactivate();
            }
        }
    }

    public override void Activate()
    {
        gameObject.SetActive(true);
        StartCoroutine(ActivatingProcess());
    }

    public IEnumerator ActivatingProcess()
    {
        while(circleCollider.radius < passiveImprovment.radius)
        {
            circleCollider.radius += passiveImprovment.timeToActivate;
            yield return null;
        }
        animator.SetTrigger("Activate");
        isActive = true;
    }

    public override void Deactivate()
    {
        animator.SetTrigger("Deactivate");
        StartCoroutine(DeactivatingProcess());
    }

    public IEnumerator DeactivatingProcess()
    {
        while(circleCollider.radius > 0)
        {
            circleCollider.radius -= passiveImprovment.timeToActivate;
            yield return null;
        }
        isActive = false;
        gameObject.SetActive(false);
    }
}
