using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiColliderTrigger : MonoBehaviour
{
    //allowed objects tags for collision detection
    public List<string> tags = new List<string>();

    //my collider
    private Collider2D col;
    //ai behavior component in parent object
    private AiBehavior aiBehavior;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        aiBehavior = GetComponentInParent<AiBehavior>();
        Debug.Assert(col && aiBehavior, "Wrong initial parameters");
        col.enabled = false;
    }
    void Start()
    {
        col.enabled = true;
    }

    private bool IsTagAllowed(string tag)
    {
        bool res = false;
        if (tags.Count>0)
        {
            foreach (string str in tags)
            {
                if (str==tag)
                {
                    res = true;
                    break;
                }
            }
        }
        else
        {
            res = true;
        }
        return res;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsTagAllowed(other.tag)==true)
        {
           // Debug.Log("enemyfind");
            //notify ai behavior about this event
            aiBehavior.OnTrigger(AiState.Trigger.TriggerEnter, col, other);
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (IsTagAllowed(other.tag) == true)
        {
            // Notify AI behavior about this event
            aiBehavior.OnTrigger(AiState.Trigger.TriggerStay, col, other);
        }
    }

    /// <summary>
    /// Raises the trigger exit2d event.
    /// </summary>
    /// <param name="other">Other.</param>
    void OnTriggerExit2D(Collider2D other)
    {
       // Debug.Log("11");
        if (IsTagAllowed(other.tag) == true)
        {
            // Notify AI behavior about this event
            aiBehavior.OnTrigger(AiState.Trigger.TriggerExit, col, other);
        }
    }
}
