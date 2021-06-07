using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiStateMove : AiState
{   
    [Space(10)]
    //end point for moving
    public Transform destination;
    //go to this state if passive event occures
    public AiState passiveAiState;

    //navigation agent of this gameobject
    NavAgent navAgent;


    public override void Awake()
    {
        base.Awake();
        navAgent = GetComponent<NavAgent>();
        Debug.Assert(navAgent, "Wrong initial parameters");
    }

    public override void OnStateEnter(AiState previousState, AiState newState)
    {
        //set destination for navigation agent
        navAgent.destination = destination.position;
        //start moving
        navAgent.move = true;
        navAgent.turn = true;
        if (anim!=null)
        {
            //play animation
            anim.SetTrigger("move");
        }
    }

    public override void OnStateExit(AiState previousState, AiState newState)
    {
        navAgent.move = false;
        navAgent.turn = false;
    }

    void FixedUpdate()
    {
        //if destination reached
        if ((Vector2)transform.position==(Vector2)destination.position)
        {
            //look at required direction
            navAgent.LookAt(destination.right);
            //go to passive state
            aiBehavior.ChangeState(passiveAiState);
        }
    }
}
