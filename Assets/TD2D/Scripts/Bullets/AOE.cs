using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE : MonoBehaviour
{
    // Percent of AOE damage in part of IBullet damage. 0f = 0%, 1f = 100%
    public float aoeDamageRate = 1f;
    // Area radius
    public float radius = 0.3f;
    // Explosion prefab
    public GameObject explosion;
    // Explosion visual duration
    public float explosionDuration = 1f;

    //iBullet componet of this gameObkject to getthe damage amount
    private IBullet bullet;
    //scene is closed now.Forbidden to create new objects on destroy
    private bool isQuiting;

    void Awake()
    {
        bullet = GetComponent<IBullet>();
        Debug.Assert(bullet != null, "Wrong initial settings");
    }

    void OnEnable()
    {
        EventManager.StartListening("SceneQuit", SceneQuit); 
    }
    void OnDisable()
    {
        EventManager.StopListening("SceneQuit", SceneQuit);    
    }

    void OnApplicationQuit()
    {
        isQuiting = true;
    }
    void OnDestroy()
    {
        //if scene is in progress
        if (isQuiting==false)
        {
            //find all colliders in specified radius
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (Collider2D col in cols)
            {
                //if target can receive damage
                DamageTaker damageTaker = col.gameObject.GetComponent<DamageTaker>();
                if (damageTaker!=null)
                {
                    //target takes damage equal bullet damage*aoe damage rate
                    //mathf.ceil 向上取整
                    damageTaker.TakeDamage((int)(Mathf.Ceil(aoeDamageRate * (float)bullet.GetDamage())));
                }
            }
            if (explosion!=null)
            {
                //create explosion visual effect
                Destroy(Instantiate<GameObject>(explosion, transform.position, transform.rotation), explosionDuration);
              
            }
        }
    }
    private void SceneQuit(GameObject obj, string param)
    {
        isQuiting = true;
    }
}
