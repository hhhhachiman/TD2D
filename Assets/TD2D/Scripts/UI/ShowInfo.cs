using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowInfo : MonoBehaviour
{
    //name of unit
    public Text unitName;
    //primary icon for displaying
    public Image primaryIcon;
    //primary text for displaying
    public Text primaryText;
    //secondary icon for displaying
    public Image secondaryIcon;
    //secondary text for displaying
    public Text secondaryText;

    void OnDestroy()
    {
        EventManager.StopListening("UserClick", UserClick);
    }

    void Awake()
    {
        Debug.Assert(unitName && primaryIcon && primaryText && secondaryIcon && secondaryText, "Wrong initial parameters");
    }
    

    void Start()
    {
        EventManager.StartListening("UserClick", UserClick);
        HideUnitInfo();
    }

    public void ShowUnitInfo(UnitInfo info)
    {
        gameObject.SetActive(true);
        unitName.text = info.unitName;
        primaryText.text = info.primaryText;
        secondaryText.text = info.secondaryText;
        if (info.primaryIcon!=null)
        {
            primaryIcon.sprite = info.primaryIcon;
            primaryIcon.gameObject.SetActive(true);
        }
        if (info.secondaryIcon!=null)
        {
            secondaryIcon.sprite = info.secondaryIcon;
            secondaryIcon.gameObject.SetActive(true);
        }
    }
    private void HideUnitInfo()
    {
        unitName.text = primaryText.text = secondaryText.text = "";
        primaryIcon.gameObject.SetActive(false);
        secondaryIcon.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void UserClick(GameObject obj, string param)
    {
        HideUnitInfo();
        if (obj!=null)
        {
            UnitInfo unitInfo = obj.GetComponent<UnitInfo>();
            if (unitInfo!=null)
            {
                ShowUnitInfo(unitInfo);
            }
        }
    }

}
