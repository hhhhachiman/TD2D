using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirStrike : MonoBehaviour
{
    //delay for FX
    public float[] delaysBeforeDamage = { 0.5f };
    //damage for each hit
    public int damage = 5;
    //damage radius
    public float radius = 1f;
    //fx prefab
    public GameObject effectPrefab;
    //after this timeout FX will be destroyed
    public float effectDuration = 2f;

    //machine state
    private enum Mystate
    {
        WaitForClick,
        WaitForFX
    }

    private Mystate myState = Mystate.WaitForClick;

    void OnEnable()
    {
        EventManager.StartListening("UserClick", UserClick);
        EventManager.StartListening("UserUiClick", UserUiClick);
    }
    void OnDisable()
    {
        EventManager.StopListening("UserClick", UserClick);
        EventManager.StopListening("UserUiClick", UserUiClick);
    }

    
    void Start()
    {
        Debug.Assert(effectPrefab, "wrong initial parameters");
    }

    private void UserUiClick(GameObject obj, string param)
    {
        //if clicked on ui instead game map
        if (myState==Mystate.WaitForClick)
        {
            Destroy(gameObject);
        }
    }

    private void UserClick(GameObject obj, string param)
    {
        if (myState==Mystate.WaitForClick)
        {
            // if clicked not on tower
            if (obj == null||obj.CompareTag("Tower")==false)
            {
                //create FX
                transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
                SoundManager.play_effect("Sounds/explo_fireend1");
                Destroy(effect, effectDuration);
                EventManager.TriggerEvent("ActionStart", gameObject, null);
                //start damage coroutine
                StartCoroutine(DamageCoroutine());

            }
            else
            {
                Destroy(gameObject);
            }

        }
    }

    private IEnumerator DamageCoroutine()
    {
        foreach(float delay in delaysBeforeDamage)
        {
            yield return new WaitForSeconds(delay);

            //search for targets
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (Collider2D col in hits)
            {
                if (col.CompareTag("Enemy") == true)
                {
                    DamageTaker damageTaker = col.GetComponent<DamageTaker>();
                    if (damageTaker != null)
                    {
                        damageTaker.TakeDamage(damage);
                    }
                }
            }
        }
        Destroy(gameObject);
    }
    void OnDestroy()
    {
        if (myState==Mystate.WaitForClick)
        {
            EventManager.TriggerEvent("ActionCancel", gameObject, null);
        }
        StopAllCoroutines();
    }


}
