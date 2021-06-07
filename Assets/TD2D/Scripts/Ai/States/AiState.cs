using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AiState : MonoBehaviour
{
    //����Ai״̬ת���Ĵ�������
    public enum Trigger
    {
        TriggerEnter,   //on collider enter
        TriggerStay,    //on collider stay
        TriggerExit,    //on collider exit
        Dameage,        //on damage taken
        Cooldown        //on some cooldown expired
    }
    //���л� Allows to specify AI state change on any trigger
    [Serializable]

    public class AiTransaction
    {
        public Trigger trigger;
        public AiState newState;
    }
    //List with specified transactions for this AI state
    public AiTransaction[] specificTransactions;

    //AI����������
    protected Animator anim;
    //��ǰ�����ai��Ϊ
    protected AiBehavior aiBehavior;

    public virtual void Awake()
    {
       aiBehavior = GetComponent<AiBehavior>();
        anim = GetComponentInParent<Animator>();
        Debug.Assert(aiBehavior, "Wrong initial parameters");
    }

    public virtual void OnStateEnter(AiState previousState, AiState newState)
    {

    }
    public virtual void OnStateExit(AiState previousState,AiState newState)
    {

    }
    /// <summary>
    /// trigger event
    /// </summary>
    /// <param name="trigger"></param>
    /// <param name="my"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public virtual bool OnTrigger(Trigger trigger,Collider2D my,Collider2D other)
    {
        bool res = false;
        //check if  this AI state has specific transactions for this trigger
        foreach(AiTransaction transaction in specificTransactions)
        {
           // Debug.Log("trigger"+trigger);
         //  Debug.Log("tttrigger" + transaction.trigger);
            if (trigger==transaction.trigger)
            {
                //Debug.Log("newstate" + transaction.newState);
                aiBehavior.ChangeState(transaction.newState);
                res = true;
                break;
            }
        }
        return res;

    } 
    

}
