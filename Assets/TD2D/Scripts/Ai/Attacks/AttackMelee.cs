
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMelee : MonoBehaviour,IAttack
{ 
    // Damage amount
    public int damage = 1;
    // Cooldown between attacks
    public float cooldown = 1f;

    // Animation controller for this AI
    private Animator anim;
    // Counter for cooldown calculation
    private float cooldownCounter;

    void Awake()
    {
        anim = GetComponentInParent<Animator>();
        cooldownCounter = cooldown;
    }

    void FixedUpdate()
    {
        if (cooldownCounter<cooldown)
        {
            cooldownCounter += Time.fixedDeltaTime;
        }
    }

    public void Attack(Transform target)
    {
        if (cooldownCounter>=cooldown)
        {
            cooldownCounter = 0f;
            Smash(target);
        }
    }

    private void Smash(Transform target)
    {
        if (target != null)
        {
            //if target can receive damage
            DamageTaker damageTaker = target.GetComponent<DamageTaker>();
            if (damageTaker!=null)
            {
                damageTaker.TakeDamage(damage);
                //AudioManager.Instance.Play("shield_fire2");
                SoundManager.play_effect("Sounds/shield_fire2");
                if (anim!=null)
                {
                    anim.SetTrigger("attackMelee");
                }
            }
        }
       
    }
}
