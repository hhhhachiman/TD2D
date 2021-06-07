
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tower : MonoBehaviour
{   //prefab for buildingtree
    public GameObject buildingTreePrefab;
    public GameObject buildingplace;
    public GameObject flagPrefab;
    public GameObject rangeImage;

    //user interface manager
    private UiManager uiManager;
    //level ui canvas for buildingtree display
    private Canvas canvas;
    //collider of this tower
    private Collider2D bodyCollider;
    //display buildingtree
    private BuildingTree activeBuildingTree;
    private Price price;
    //defender tower moving check
    private Ray ray;
    private RaycastHit hit;
    //private AudioManager audioManager;
  
    private GameObject activeflag;
    private DefendersSpawner defendersSpawner;
    void OnEnable()
    {
        EventManager.StartListening("GamePaused", GamePaused);
        EventManager.StartListening("UserClick", UserClick);
        EventManager.StartListening("UserUiClick", UserClick);
    }

    

    void OnDisable()
    {
        EventManager.StopListening("GamePaused", GamePaused);
        EventManager.StopListening("UserClick", UserClick);
        EventManager.StopListening("UserUiClick", UserClick);
    }
    void Start()
    {
        uiManager = FindObjectOfType<UiManager>();
        //this canvas will use to place buidingtree ui
        Canvas[] canvases = Resources.FindObjectsOfTypeAll<Canvas>();
        foreach (Canvas canv in canvases)
        {
            if (canv.CompareTag("LevelUI"))
            {
                canvas = canv;
                break;
            }
        }
        bodyCollider = GetComponent<Collider2D>();
        Debug.Assert(uiManager && canvas && bodyCollider, "Wrong initial parameters");
    }

    private void UserClick(GameObject obj, string param)
    {   //this tower is clicked
       // Debug.Log(obj.name) ;
        if (obj == gameObject)
        {
            //show attack range
            Showrange(true);
            if (activeBuildingTree == null)
            {
                //open building tree if it is not
                OpenBuildingTree();
            }
            
        }
        //else if (obj.name == "Range")
        //{
        //    Showrange(true);
        //    Debug.Log("click on flag listening!");
        //    ControllDefender();

        //}
        else
        {
            Showrange(false);
            CloseBuildingTree();
        }
    }
    private void OpenBuildingTree()
    {
        if (buildingTreePrefab!=null)
        {
            //create buildingtree
            activeBuildingTree = Instantiate<GameObject>(buildingTreePrefab, canvas.transform).GetComponent<BuildingTree>();
            //set it over the tower
            activeBuildingTree.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            activeBuildingTree.myTower = this;
            bodyCollider.enabled = false;

        }
    }
    private void CloseBuildingTree()
    {
        if (activeBuildingTree!=null)
        {
            Destroy(activeBuildingTree.gameObject);
            bodyCollider.enabled = true;
        }
    }

    public void  BuildTower(GameObject towerprefab)
    {
        
       // Debug.Log("tower");
        //close active buildingtree
        CloseBuildingTree();
         price = towerprefab.GetComponent<Price>();
        if (uiManager.SpendGold(price.price)==true)
        {
            //create new tower and set it on the same position
            GameObject newTower = Instantiate<GameObject>(towerprefab, transform.parent);
            newTower.transform.position = transform.position;
            newTower.transform.rotation = transform.rotation;
            Destroy(gameObject);
            Debug.Log("hh" + gameObject);
            //AudioManager.Instance.Play("building");
            SoundManager.play_effect("Sounds/building");
            string tname = System.Text.RegularExpressions.Regex.Replace(newTower.name, @"[0-9]+", "");
            Debug.Log(tname);
            switch (tname)
            {
                case "BarracksL(Clone)":
                    SoundManager.play_effect("Sounds/shield_ready1");
                    break;
                case "BowmanL(Clone)":
                    SoundManager.play_effect("Sounds/arrow_ready2");
                    break;
                case "BallistaL(Clone)":
                    SoundManager.play_effect("Sounds/arrow_ready1");

                    break;
                case "MagicL(Clone)":
                    SoundManager.play_effect("Sounds/magic_ready3");
                    break;
                case "MortarL(Clone)":
                    SoundManager.play_effect("Sounds/explo_ready1");
                    break;
            }

        }
    }

    public void SellingTower(GameObject towerprefab)
    {
         CloseBuildingTree();
         EventManager.TriggerEvent("SellTower",gameObject, null);
        //Debug.Log("Towerprice  " + price);
       if (towerprefab!=null)
        {
            Debug.Log("ddddestorytower  "+gameObject.name );
           
            GameObject bdplace = Instantiate<GameObject>(towerprefab, transform.parent);
            bdplace.transform.position = transform.position;
            bdplace.transform.rotation = transform.rotation;
            //GameObject buildinitial = Instantiate<GameObject>(buildingplace, transform.parent);
            //buildinitial.transform.position = transform.position;
            //buildinitial.transform.rotation = transform.rotation;
            Destroy(gameObject);
            //gameObject.SetActive(false);

        }
        else
        {
          Debug.Log("no gameobject");
       }

    }

    private void Showrange(bool condition)
    {
        if (rangeImage != null)
        {
            rangeImage.SetActive(condition);
        }
    }
    private void GamePaused(GameObject obj, string param)
    {
        //将布尔值 true 表示为字符串。此字段为只读。该字段等于字符串“True”。
        if (param==bool.TrueString)
        {
            CloseBuildingTree();
            bodyCollider.enabled = false;
        }
        else
        {
            bodyCollider.enabled = true;
        }
    }
    //public void SetFlag(bool flaged)
    //{
    //    setflag = flaged;
    //}
    //public void ControllDefender()
    //{
    //    CloseBuildingTree();
    //    Showrange(true);
    //    Vector3 hit = uiManager.GetMousePoisition();
    //     Debug.Log("hit.position"+ hit.x+"   "+hit.y);
    //    activeflag = Instantiate<GameObject>(flagPrefab, hit.x, hit.y);
       

    //}

}
