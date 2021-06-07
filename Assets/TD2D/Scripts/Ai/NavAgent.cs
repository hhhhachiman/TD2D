using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavAgent : MonoBehaviour
{
    //速度
    public float speed = 1f;
    // can move
    [HideInInspector]
    public bool move= true;
    //turning
    [HideInInspector]
    public bool turn = true;
    //destination position
    [HideInInspector]
    public Vector2 destination;
    //Vector velocity
    [HideInInspector]
    public Vector2 velocity;

    // present position
    private Vector2 prevPosition;

     void OnEnable()
    {
        //present position
        prevPosition = transform.position;

    }

    private void FixedUpdate()
    {
        //if moving is allowed
        if (move==true)
        {
            //move towards the destination
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.fixedDeltaTime);
        }
        //calculate velocity
        //Vector2 velocity = (Vector2)transform.position - prevPosition;
        //velocity /= Time.fixedDeltaTime;
        //if turning is allowed
        if (turn==true)
        {
          // SetSpriteDirection(destination - (Vector2)transform.position);
        }
        prevPosition = transform.position;
    }

    //做好动画好好研究转向部分
    private void SetSpriteDirection(Vector2 direction)
    {
        if (direction.x > 0f && transform.localScale.x < 0f) //to the right
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0f && transform.localScale.x >0f) //to the left
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    public void LookAt(Vector2 direction)
    {
        SetSpriteDirection(direction);
    }
    public void  LookAt(Transform traget)
    {
        SetSpriteDirection(traget.position - transform.position);
    }

    
}
