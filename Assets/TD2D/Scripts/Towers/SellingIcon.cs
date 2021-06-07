using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellingIcon : MonoBehaviour
{
    public GameObject tower;
    //public GameObject buildingtree;
    private Price  price;
    private BuildingTree myTree;
    void OnEnable()
    {
        EventManager.StartListening("UserUiClick", UserUiClick);
    }

  

    void OnDisable()
    {
        EventManager.StopListening("UserUiClick", UserUiClick);
    }
    void Awake()
    {
        myTree = transform.GetComponentInParent<BuildingTree>();
        //price = tower.GetComponent<Price>();
        
        //ebug.Log("towerprice" + price);
        Debug.Assert( myTree, "wrong initial parameters");
        
       
    }

    private void UserUiClick(GameObject obj, string param)
    {
        if (obj == gameObject)
        {
            myTree.Sell(tower);
            Debug.Log("Usersell");
        }
    }

}
