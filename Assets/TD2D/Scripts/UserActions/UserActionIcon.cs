using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserActionIcon : MonoBehaviour
{
    public float cooldown = 10f;
    public GameObject userActionPrefab;
    public GameObject highlightIcon;
    public GameObject cooldownIcon;
    public Text cooldownText;
    private enum MyState
    {
        Active,
        Highligted,
        Cooldown
    }

    //current state for this instance
    private MyState myState = MyState.Active;
    //active user action,instantiated when highlited
    private GameObject activeUserAction;
    //counter for cooldown
    private float cooldonCounter;



    void OnEnable()
    {
        EventManager.StartListening("UserUiClick", UserUiClick);
        EventManager.StartListening("ActionStart", ActionStart);
        EventManager.StartListening("ActionCancel", ActionCancel);
    }

    void OnDisable()
    {
        EventManager.StopListening("UserUiClick", UserUiClick);
        EventManager.StopListening("ActionStart", ActionStart);
        EventManager.StopListening("ActionCancel", ActionCancel);
    }
    void Start()
    {
        Debug.Assert(userActionPrefab && highlightIcon && cooldownIcon && cooldownText, "Wrong initial settings");
        StopCooldown();

    }

    void Update()
    {
      if (myState == MyState.Cooldown)
        {
            if (cooldonCounter>0f)
            {
                cooldonCounter -= Time.deltaTime;
                UpdateCooldownText();
            }
            else if (cooldonCounter<=0f)
            {
                StopCooldown();
            }
        }
    }

    private void UpdateCooldownText()
    {
        cooldownText.text = ((int)Mathf.Ceil(cooldonCounter)).ToString();
    }

    private void StopCooldown()
    {
        myState = MyState.Active;
        cooldonCounter = 0f;
        cooldownIcon.gameObject.SetActive(false);
        cooldownText.gameObject.SetActive(false);
    }

    private void ActionCancel(GameObject obj, string param)
    {
        if (obj==activeUserAction)
        {
            activeUserAction = null;
            highlightIcon.SetActive(false);
            myState = MyState.Active;
        }
    }

    private void ActionStart(GameObject obj, string param)
    {
        if (obj==activeUserAction)
        {
            activeUserAction = null;
            highlightIcon.SetActive(false);
            StartCooldown();
        }
    }

    private void StartCooldown()
    {
        myState = MyState.Cooldown;
        cooldonCounter = cooldown;
        cooldownIcon.gameObject.SetActive(true);
        cooldownText.gameObject.SetActive(true);
    }

    private void UserUiClick(GameObject obj, string param)
    {
        if (obj == gameObject)  //clicked on this icon
        {
            if (myState==MyState.Active)
            {
                highlightIcon.SetActive(true);
                activeUserAction = Instantiate(userActionPrefab);
                myState = MyState.Highligted;

            }   
        }
        else if (myState==MyState.Highligted)
        {
            highlightIcon.SetActive(false);
            myState = MyState.Active;
        }
    }

   
   

  
}
