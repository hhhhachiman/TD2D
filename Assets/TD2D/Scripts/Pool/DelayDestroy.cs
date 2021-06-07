using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
   public void Init()
    {
        StartCoroutine(ReturnToPOol());
    }

    IEnumerator ReturnToPOol()
    {
        yield return new WaitForSeconds(2f);
        GameObjectPool.instance.IntoPool(this.gameObject);
    }
}
