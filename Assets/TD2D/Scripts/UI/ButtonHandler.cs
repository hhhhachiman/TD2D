using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public void ButtonPressed(string buttonName)
    {
        //send global event about button pressing
        EventManager.TriggerEvent("ButtonPressed", gameObject, buttonName);
    }
}
