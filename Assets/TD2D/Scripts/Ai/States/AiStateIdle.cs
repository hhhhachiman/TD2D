using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiStateIdle : AiState
{
    
    public override void OnStateEnter(AiState previousState,AiState newState)
    {
        if (anim!=null)
        {
            anim.SetTrigger("idle");
        }
    }
}
