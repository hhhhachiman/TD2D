using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllingIcon : MonoBehaviour
{

    private BuildingTree myTree;


    void OnEnable()
    {
        EventManager.StartListening("UserUiClick", UserUiClick);
    }

    /// <summary>
    /// Raises the disable event.
    /// </summary>
    void OnDisable()
    {
        EventManager.StopListening("UserUiClick", UserUiClick);
    }

    private void UserUiClick(GameObject obj, string param)
    {
        //  if (obj == gameobject)
        //{
        //     mytree.controll(true);
        //        debug.log("clickonflag£¡");
        //   }
    }

    void Awake()
    {
        myTree = transform.GetComponentInParent<BuildingTree>();
        Debug.Assert(myTree, "wrong initial parameters");

    }

   
}
