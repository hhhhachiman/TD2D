using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBold : MonoBehaviour, IBullet
{
    //damage amount
    [HideInInspector] public int damage = 1;
    //maximum life time
    public float lifeTime = 3f;
    //starting speed
    public float speed = 3f;
    //constant acceleration
    public float speedUpOverTime = 0.5f;
    //if target is close than this distance,it will be hitted
    public float hitDistance = 0.2f;
    // Ballistic trajectory offset (in distance to target)
    public float ballisticOffset = 0.1f;
    // Bullet will fly trough target (in origin distance to target)
    public float penetrationRatio = 0.3f;

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
    //image of this sprite
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
        sprite = GetComponent<SpriteRenderer>();
        //disable sprite on first frame becz we do not know fly direction yet
        sprite.enabled = false;
        originPoint = myVirtualPosition = myPreviousPosition = transform.position;
        this.target = target;
        aimPoint = GetPenetrationPoint(target.position);
        //destroy bullet after lifetime
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        counter += Time.fixedDeltaTime;
        //add acceleration
        speed += Time.fixedDeltaTime * speedUpOverTime;
        if (target != null)
        {
            aimPoint = GetPenetrationPoint(target.position);
        }
        //calculate distance from firpoint to aim
        Vector2 originDistance = aimPoint - originPoint;
        //calculate remaining distance
        Vector2 distanceToAim = aimPoint - (Vector2)myVirtualPosition;
        //move towards aim
        myVirtualPosition = Vector2.Lerp(originPoint, aimPoint, counter * speed / originDistance.magnitude);
        //add ballistic towards trajectory
        transform.position = AddBallisticOffset(originDistance.magnitude, distanceToAim.magnitude);
        //rotate bullet towards trajectory
        LookAtDirection2D((Vector2)transform.position - myPreviousPosition);
        myPreviousPosition = transform.position;
        sprite.enabled = true;
        // Close enough to hit
        if (distanceToAim.magnitude <= hitDistance)
        {
            // Destroy bullet
            Destroy(gameObject);
        }



    }

    private void LookAtDirection2D(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private Vector2 AddBallisticOffset(float originDistance, float distanceToAim)
    {
        if (ballisticOffset > 0f)
        {
            // Calculate sinus offset
            float offset = Mathf.Sin(Mathf.PI * ((originDistance - distanceToAim) / originDistance));
            offset *= originDistance;
            // Add offset to trajectory
            return (Vector2)myVirtualPosition + (ballisticOffset * offset * Vector2.up);
        }
        else
        {
            return myVirtualPosition;
        }
    }

    private Vector2 GetPenetrationPoint(Vector2 targetPosition)
    {
        Vector2 penetrationVector = targetPosition - originPoint;
        return originPoint + penetrationVector * (1f + penetrationRatio);
    }
    void onTriggerEnter2D(Collider2D other)
    {
        //if target can receive damage
        DamageTaker damageTaker = other.GetComponent<DamageTaker>();
        if (damageTaker!=null)
        {
            damageTaker.TakeDamage(damage);
        }
    }
}
