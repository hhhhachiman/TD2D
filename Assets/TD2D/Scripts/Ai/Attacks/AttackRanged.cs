using Lemon.audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRanged : MonoBehaviour,IAttack
{
    //Damage amount
    public int damage = 1;
    //cooldown between attacks;
    public float cooldown = 1f;
    //prefab for arrows;
    public GameObject arrowPrefab;
    //from this position arrows will fired
    public Transform firePoint;

    //Animation controller for this AI
    private Animator anim;
    //counter for cooldownCounter;
    private float cooldownCounter;

    void Awake()
    {
        anim = GetComponentInParent<Animator>();
        cooldownCounter = cooldown;
        Debug.Assert(arrowPrefab && firePoint, "Wrong initial parameters");
        
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
        //Debug.Log("cooldownCounter  "+cooldownCounter + "cooldown  "+cooldown);
        if (cooldownCounter>=cooldown)
        {
            cooldownCounter = 0f;
            Fire(target);
        }
    }
    private void Fire(Transform target)
    {
       
        if (target!=null)
        {
            
            //create arrow
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
            IBullet bullet = arrow.GetComponent<IBullet>();
            bullet.SetDamage(damage);
            bullet.Fire(target);
           
            switch (arrowPrefab.name)
            {
                case "Arrow":
                    SoundManager.play_effect("Sounds/arrow_fire1");
                   // AudioManager.Instance.Play("arrow_fire1");
                    break;
                case "MagicMissile":
                    SoundManager.play_effect("Sounds/magic_fire1");
                    //AudioManager.Instance.Play("magic_fire1");
                    break;
                case "BombAlly":
                    //AudioManager.Instance.Play("explo_firestart1");
                    SoundManager.play_effect("Sounds/explo_firestart1");
                    break;
                case "Bullet":    
                    //AudioManager.Instance.Play("explo_firestart1");
                    SoundManager.play_effect("Sounds/explo_firestart1");
                    break;
                case "Bold":
                    //AudioManager.Instance.Play("arrow_fire2");
                    SoundManager.play_effect("Sounds/arrow_fire2");
                    break;
                case "Dart":
                    //AudioManager.Instance.Play("arrow_fire2");
                    SoundManager.play_effect("Sounds/arrow_fire1");
                    break;
            }

            if (anim!=null)
            {
                anim.SetTrigger("attackRanged");
            }

        }
    }
}
