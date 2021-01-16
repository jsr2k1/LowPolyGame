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
    public string sleep;
    public string walk;
    public float time_sleep = 4f;
    public float time_stand = 12f;
    public float time_walk_1 = 4f;
    public float time_walk_2 = 0.25f;

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
            animator.SetBool(walk, false);
            currentState = States.IDLE;
            StartCoroutine(GoToSleep());
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    IEnumerator GoToSleep()
    {
        yield return new WaitForSeconds(time_sleep);
        animator.SetBool(sleep, true);
        StartCoroutine(GoToStandUp());
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    IEnumerator GoToStandUp()
    {
        yield return new WaitForSeconds(time_stand);
        transform.LookAt(points[destPoint]);
        animator.SetBool(sleep, false);
        StartCoroutine(GoToWalk());
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    IEnumerator GoToWalk()
    {
        yield return new WaitForSeconds(time_walk_1);
        animator.SetBool(walk, true);
        yield return new WaitForSeconds(time_walk_2);
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
