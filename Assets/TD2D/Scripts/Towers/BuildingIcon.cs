using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingIcon : MonoBehaviour
{   //towerprefab for this icon
    public GameObject towerPrefab;
    private Text price;
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
        //Debug.Log("buildingIConAwake");
        myTree = transform.GetComponentInParent<BuildingTree>();
        price = GetComponentInChildren<Text>();
        Debug.Assert(price && myTree, "wrong initial parameters");
        if (towerPrefab==null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            price.text = towerPrefab.GetComponent<Price>().price.ToString();
        }
    }

    private void UserUiClick(GameObject obj, string param)
    {
       
        //Debug.Log("obj " + obj + " : " + "gameObject " + gameObject);   
        if (obj == gameObject)
        {
           
            myTree.Build(towerPrefab);
           
        }
    }
}
