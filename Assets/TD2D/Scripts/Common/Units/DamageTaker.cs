using Lemon.audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTaker : MonoBehaviour
{   //start hitpoints
    
    public int hitpoints = 1;
    //remaining hitpoints
    [HideInInspector]
    public int currentHitpoints;
    //Damage visual effect duration
    public float damageDisplayTime = 0.2f;
    //health bar object
    public Transform healthBar;
    //sendMessage will trigger on damage taken
    public bool isTrigger;

    //image of this object
    private SpriteRenderer sprite;
    //visualisation of hit or heal is in progress
    private bool coroutineInProgress;
    //original width of health bar
    private float originHealthBarWidth;

    void Awake()
    {
        currentHitpoints = hitpoints;
        sprite = GetComponentInChildren<SpriteRenderer>();
        Debug.Assert(sprite && healthBar, "Wrong initial parameters");
    }

    void Start()
    {
        originHealthBarWidth = healthBar.localScale.x; 
    }
    public void TakeDamage(int damage)
    {  
        if (damage>0)
        {
            if (this.enabled == true)
            {
                if (currentHitpoints>damage)
                {
                    //still alive
                    currentHitpoints -= damage;
                    UpdateHealthBar();
                    //if no coroutine now
                    if (coroutineInProgress==false)
                    {
                        //damage visualisation
                        StartCoroutine(DisplayDamage());
                    }
                    if (isTrigger==true)
                    {
                        //notify other components of this game object
                        SendMessage("OnDamage");
                    }
                }
                else
                {
                    //die
                    currentHitpoints = 0;
                    UpdateHealthBar();
                    Die();
                }
            }
            
        }
        else
        {
            //damage<0,healed
            currentHitpoints = Mathf.Min(currentHitpoints - damage, hitpoints);
            UpdateHealthBar();

        }
    }

    public void Die()
    {
        EventManager.TriggerEvent("UnitKilled", gameObject, null);
        EventManager.TriggerEvent("UnitDie", gameObject, null);
        //GameObjectPool.instance.IntoPool(this.gameObject);
        Debug.Log("return pool");
        //Destroy(gameObject);
       // EventManager.TriggerEvent("UnitDie", gameObject, null);
            StartCoroutine(ReturnToPOol());

        SoundManager.play_effect("Sounds/monster_die1");
        //AudioManager.Instance.Play("monster_die1");
    }
    IEnumerator ReturnToPOol()
    {
        bool IsPoolfilled = GameObjectPool.instance.GetPoolSize(this.gameObject);

        if (IsPoolfilled == true)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
            yield return new WaitForSeconds(2f);
            GameObjectPool.instance.IntoPool(this.gameObject);
        }
       
    }
    IEnumerator DisplayDamage()
    {
        coroutineInProgress = true;
        Color originColor = sprite.color;
        float counter;
        //set color to black and return to origin color over time
        for (counter=0f; counter<damageDisplayTime;counter+=Time.fixedDeltaTime)
        {
            sprite.color = Color.Lerp(originColor, Color.black, Mathf.PingPong(counter, damageDisplayTime));
            yield return new WaitForFixedUpdate();
        }
        sprite.color = originColor;
        coroutineInProgress = false;
    }
    private void UpdateHealthBar()
    {
        float healthBarWidth = originHealthBarWidth * currentHitpoints / hitpoints;
        healthBar.localScale = new Vector2(healthBarWidth, healthBar.localScale.y);
    }

    void OnDestroy()
    {
       // EventManager.TriggerEvent("UnitDie", gameObject, null);
        StopAllCoroutines();
    }
}
