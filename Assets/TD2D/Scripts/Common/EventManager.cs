using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyEvent : UnityEvent<GameObject, string>
{

}
public class EventManager : MonoBehaviour
{  //��������
    public static EventManager instance;
    //�¼��ֵ�
    private Dictionary<string, MyEvent> eventDictionary = new Dictionary<string, MyEvent>();


     void OnDestroy()
    {
        instance = null;    
    }

    public static void StartListening(string eventName,UnityAction<GameObject,string> listener)
    {
        if (instance==null)
        {
            instance = FindObjectOfType(typeof(EventManager)) as EventManager;
            if (instance == null)
            {
                Debug.Log("Have no event manager on scene");
                return;
            }
        }
        MyEvent thisEvent = null;
        //��ȷ��Ŀ���Ƿ���ڣ�ʹ��TryGetValue()
        if (instance.eventDictionary.TryGetValue(eventName,out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new MyEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName,UnityAction<GameObject,string> listener)
    {
        if (instance==null)
        {
            return;
        }
        MyEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName,out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName,GameObject obj,string param)
    {
        if (instance == null)
        {
            return;
        }
        MyEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(obj, param);
        }
    }
}
