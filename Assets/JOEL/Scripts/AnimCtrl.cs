using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimCtrl : MonoBehaviour
{
    public enum States {
        IDLE,
        WALK
    }
    public States currentState;
    public Transform[] points;
    
    int destPoint = 0;
    NavMeshAgent agent;
    Animator animator;

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        currentState = States.IDLE;
        StartCoroutine(GoToSleep());
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void Update()
    {
        if(currentState == States.WALK && !agent.pathPending && agent.remainingDistance < 0.5f) {
            animator.SetBool("isWalking", false);
            currentState = States.IDLE;
            StartCoroutine(GoToSleep());
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    IEnumerator GoToSleep()
    {
        yield return new WaitForSeconds(4f);
        animator.SetBool("isSleeping", true);
        StartCoroutine(GoToStandUp());
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    IEnumerator GoToStandUp()
    {
        yield return new WaitForSeconds(12f);
        transform.LookAt(points[destPoint]);
        animator.SetBool("isSleeping", false);
        StartCoroutine(GoToWalk());
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    IEnumerator GoToWalk()
    {
        yield return new WaitForSeconds(4f);
        animator.SetBool("isWalking", true);
        yield return new WaitForSeconds(0.25f);
        currentState = States.WALK;
        GotoNextPoint();
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void GotoNextPoint()
    {
        if(points.Length == 0) {
            return;
        }
        agent.destination = points[destPoint].position;
        destPoint = (destPoint + 1) % points.Length;
    }
}
