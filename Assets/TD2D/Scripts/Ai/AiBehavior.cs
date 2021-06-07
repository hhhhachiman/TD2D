using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBehavior : MonoBehaviour
{
    // This state will be activate on start
    public AiState defaultState;

    //��ǰAi����������
    private List<AiState> aiStates = new List<AiState>();
    //��ǰ״̬
    private AiState previousState;
    //��ǰ״̬
    private AiState currentState;


     void Start()
    {
        //��ȡ��ǰ��Ϸ��������AI״̬
        AiState[] states = GetComponents<AiState>();
        if (states.Length>0)
        {
            foreach(AiState state in states)
            {
                aiStates.Add(state);
            }
            if (defaultState!=null)
            {
                // Set active and previous states as default state
                previousState = currentState = defaultState;
                if (currentState!=null)
                {
                    //����
                    ChangeState(currentState);
                }
                else
                {
                    Debug.LogError("Incorrect default AI state" + defaultState);
                }
            }
            else
            {
                Debug.LogError("Ai have no default state");
            }
        }
        else
        {
            Debug.LogError("No AI states found");
        }   
    }
    /// <summary>
    /// set AI to default state
    /// </summary>
    public void GoToDefaultState()
    {
        previousState = currentState;
        currentState = defaultState;
        NotifyOnStateExit();
        DisableAllStates();
        EnableNewState();
        NotifyOnStateEnter();
    }
    public void ChangeState(AiState state)
    {
        if (state!=null)
        {
            //Debug.Log("changestate-stat!");
            //try to find such state in list
            foreach (AiState aiState in aiStates)
            {
                if (state == aiState)
                {
                    previousState = currentState;
                    currentState = aiState;
                    NotifyOnStateExit();
                    DisableAllStates();
                    EnableNewState();
                    NotifyOnStateEnter();
                    return;

                }
            }
            Debug.Log("No such state" + state);
            GoToDefaultState();
            Debug.Log("Go to the default state" + aiStates[0]);
        }
    }

    public void OnTrigger(AiState.Trigger trigger,Collider2D my, Collider2D other) 
    {
        
        if (currentState==null)
        {
            Debug.Log("Current state is null");
        }
        currentState.OnTrigger(trigger, my, other);
    }
    private void EnableNewState()
    {
        currentState.enabled = true;
    }

    private void DisableAllStates()
    {
        foreach (AiState aiState in aiStates)
        {
            aiState.enabled = false;
        }
    }

    private void NotifyOnStateEnter()
    {
        previousState.OnStateEnter(previousState, currentState);
    }
    private void NotifyOnStateExit()
    {
        previousState.OnStateExit(previousState, currentState);
    }

    
}
