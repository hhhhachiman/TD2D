using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletArrow : MonoBehaviour,IBullet
{
    //damage amount
    [HideInInspector]
    int damage = 1;
    //maximum life time
    public float lifeTime = 3f;
    //starting speed
    public float speed = 3f;
    //con stant acceleration
    public float speedUpOverTime = 0.5f;
    //if target is close than this distance-it will be hitted
    public float hitDistance = 0.1f;
    //ballistic trajectory offset (in distance to target)
    public float ballisticOffset = 0.5f;
    //donot dotate bullet during fly
    public bool freezeRotation = false;
    //this bullet dont't deal damage to single target.only aoe damage if it is
    public bool aoeDamageOnly = false;

    //from this position bullet was fired
    private Vector2 originPoint;
    //aimed target
    private Transform target;
    //last target's position
    private Vector2 aimPoint;
    //current position without ballistic offset
    private Vector2 myVirtualPosition;
    //position on last frame
    private Vector2 myPreviousPosition;
    //counter for acceleration calculation
    private float counter;
    //image of this bullet
    private SpriteRenderer sprite;


    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public int GetDamage()
    {
        return damage;
    }

    public void Fire(Transform target)
    {
         //Debug.Log("Fire");
        sprite = GetComponent<SpriteRenderer>();
        //disable sprite on first frame bcuz we dont know fly direction yet
        sprite.enabled = false;
        originPoint = myVirtualPosition = myPreviousPosition = transform.position;
        this.target = target;
        aimPoint = target.position;
        //destroy bullet after lifetime
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        counter += Time.fixedDeltaTime;
        //add acceleration
        speed += Time.fixedDeltaTime * speedUpOverTime;
        if (target!=null)
        {
            aimPoint = target.position;
        }
        //caculate distance from firepoint to aim
        Vector2 originDistance = aimPoint - originPoint;
        //cacu remaining distance
        Vector2 distanceToAim = aimPoint - (Vector2)myVirtualPosition;
        //move towards aim use lerp to make moving smooth
        myVirtualPosition = Vector2.Lerp(originPoint, aimPoint, counter * speed / originDistance.magnitude);
        //add ballistic offset to trajectory
        transform.position = AddBallisticOffset(originDistance.magnitude, distanceToAim.magnitude);
        //rotate bullet towards trajectory
        LookAtDirection2D((Vector2)transform.position - myPreviousPosition);
        myPreviousPosition = transform.position;
        sprite.enabled = true;
        //close enough to hit
        if (distanceToAim.magnitude<=hitDistance)
        {
            if (target!=null)
            {
                //if bullet must deal damage to single target
                if (aoeDamageOnly==false)
                {
                    //if target can receive damage
                    DamageTaker damageTaker = target.GetComponent<DamageTaker>();
                    if (damageTaker!=null)
                    {
                        damageTaker.TakeDamage(damage);
                    }
                }
            }
            Destroy(gameObject);
           
            //ArrowPool.ReturnInstance(,originPoint);
        }
    }

    private void LookAtDirection2D(Vector2 direction)
    {
        if (freezeRotation==false)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private Vector2 AddBallisticOffset(float originDistance, float distanceToAim)
    {
        if (ballisticOffset>0f)
        {
            //caculate sinus offset
            float offset = Mathf.Sin(Mathf.PI * ((originDistance - distanceToAim) / originDistance));
            offset *= originDistance;
            //add offset to trajectory
            return (Vector2)myVirtualPosition + (ballisticOffset * offset * Vector2.up);
        }
        else
        {
            return myVirtualPosition;
        }
    }
}
