using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTree : MonoBehaviour
{
    [HideInInspector]
    public Tower myTower;
    void Start()
    {
        //Debug.Log("buildingtreeStart");
        Debug.Assert(myTower, "Wrong initial parameters");
    }

    public void Build(GameObject prefab)
    {
        //Debug.Log("buildingtree");
        myTower.BuildTower(prefab);
        
    }

    public void Sell(GameObject placeprefab)
    {
        //EventManager.TriggerEvent("SellTower",placeprefab, null);
        myTower.SellingTower(placeprefab);
        //Debug.Log("treesell");
    }

    //public void Controll(bool flag)
    //{
    //    myTower.SetFlag(flag);
    //    myTower.ControllDefender();
    //    Debug.Log("BDTtoTOWER!  " + flag);
    //}

}
