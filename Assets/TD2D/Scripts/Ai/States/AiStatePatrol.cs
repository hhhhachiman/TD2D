using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ai 沿指定路线行走
/// </summary>
public class AiStatePatrol : AiState
{
    [Space(10)]
    [HideInInspector]
    public Pathway path;

    public bool loop = false;

    //导航向导
    NavAgent navAgent;
    //current destination;
    private Waypoint destination;

    /// <summary>
    ///重写Aistate的方法
    ///初始化实例
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        navAgent = GetComponent<NavAgent>();
        Debug.Assert(navAgent, "Wrong initial parameters");
    }

    public override void OnStateEnter(AiState previousState, AiState newState)
    {
        if (path ==null)
        {
            //if we have no path,try to find it
            path = FindObjectOfType<Pathway>();
            Debug.Assert(path, "Have no path");
        }
        if (destination==null)
        {
            //get next waypoint from my path
            destination = path.GetNearestWayPoint(transform.position);
        }
        navAgent.destination = destination.transform.position;
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
        //stop moving
        navAgent.move = false;
        navAgent.turn = false;
    }

     void FixedUpdate()
    {
        if (destination!=null)
        {
            //if destination reached
            if ((Vector2)destination.transform.position==(Vector2)transform.position)
            {
                //get next waypoint from my path
                destination = path.GetNextWayPoint(destination, loop);
                if (destination!=null)
                {
                    //set destinnation for navigation agent
                    navAgent.destination = destination.transform.position;
                }
            }    
        }   
    }

      public float GetRemainingPath()
    {
        Vector2 distance = destination.transform.position - transform.position;
        return (distance.magnitude + path.GetPathDistance(destination));
    }

}
