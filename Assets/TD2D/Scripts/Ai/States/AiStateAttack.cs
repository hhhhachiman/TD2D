using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiStateAttack : AiState
{
    [Space(10)]
    //attack target closest to the capture point
    public bool useTargetPriority = false;
    //go to this state if passive event occures
    public AiState passiveAiState;

    //target for attack
    
    private GameObject target;
    //list with potential targets finded during this frame
    private List<GameObject> targetsList = new List<GameObject>();
    //melee attack type
    private IAttack meleeAttack;
    //ranged attack type
    private IAttack rangedAttack;
    //last attack is made
    private IAttack myLastAttack;
    NavAgent nav;
    //allows to await new target for one frame before exit from this state
    private bool targetless;

    public override void Awake()
    {
        base.Awake();
        meleeAttack = GetComponentInChildren<AttackMelee>() as IAttack;
        rangedAttack = GetComponentInChildren<AttackRanged>() as IAttack;
        nav = GetComponent<NavAgent>();
        Debug.Assert(meleeAttack != null || rangedAttack != null, "Wrong initial parameters");
    }

    public override void OnStateExit(AiState previousState, AiState newState)
    {
        LoseTarget();
    }

    void FixedUpdate()
    {
        //if have no target,try to find new target
        if ((target==null)&&(targetsList.Count>0))
        {
            target = GetTopmostTarget();
            if ((target!=null)&&(nav!=null))
            {
                nav.LookAt(target.transform);
            }
        }
        //There are no targets around
        if (target==null)
        {
            if (targetless==false)
            {
                targetless = true;
            }
            else
            {
                //if have no target  more than one frame ,exit from this state
                aiBehavior.ChangeState(passiveAiState);
            }
        }
    }


    private GameObject GetTopmostTarget()
    {
        GameObject res = null;
        if (useTargetPriority == true)
        {
            float minPathDistance = float.MaxValue;
            foreach (GameObject ai in targetsList)
            {
                if (ai != null)
                {
                    AiStatePatrol aiStatePatrol = ai.GetComponent<AiStatePatrol>();
                    float distance = aiStatePatrol.GetRemainingPath();
                    if (distance < minPathDistance)
                    {
                        minPathDistance = distance;
                        res = ai;
                    }
                }
            }
        }
        else
        //get first target from list
        {
            res = targetsList[0];
        }
        //clear list of potential targets
        targetsList.Clear();
        return res;
    }

    private void LoseTarget()
    {
        target = null;
        targetless = false;
        myLastAttack = null;
    }

    public override bool OnTrigger(Trigger trigger, Collider2D my, Collider2D other)
    {
        if (base.OnTrigger(trigger,my,other)==false)
        {
            switch (trigger)
            {
                case AiState.Trigger.TriggerStay:
                    //Debug.Log("TriggerStay1");
                    TriggerStay(my, other);
                    break;
                case AiState.Trigger.TriggerExit:
                    //Debug.Log("Triggerexit1");
                    TriggerExit(my, other);
                    break;
            }
        }
        return false;
    }
    private void TriggerStay(Collider2D my, Collider2D other)
    {  //add new target to potential targets list
        if (target==null)
        {
            targetsList.Add(other.gameObject);
        }
        else
        //attack current target
        {
      
            //if this is current target
            if (target==other.gameObject)
            { //if target if in melee attack range
               // Debug.Log("myname000" + my.name);
                if (my.name=="MeleeAttack")
                {
                    if (meleeAttack != null)
                    {
                       
                        //remember my last attack type
                        myLastAttack = meleeAttack as IAttack;
                        //try to make melee attack
                        meleeAttack.Attack(other.transform);

                    }
                }
                //if target is in ranged attack range
                else if(my.name == "RangedAttack")
                {
                    if (rangedAttack!=null)
                    {   //if target not in melee attack range
                        if ((meleeAttack==null)||(meleeAttack!=null)&&(myLastAttack!=meleeAttack))
                        {
                            //remember my last attack type
                            myLastAttack = rangedAttack as IAttack;
                            //try to make ranged attack
                            rangedAttack.Attack(other.transform);
                        }

                    }
                }
            }

        }
    }
    private void TriggerExit(Collider2D my, Collider2D other)
    {
        if (other.gameObject==target)
        {
            LoseTarget();
        }
    }

   
}
