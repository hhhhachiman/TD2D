using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyEvent : UnityEvent<GameObject, string>
{

}
public class EventManager : MonoBehaviour
{  //创建单例
    public static EventManager instance;
    //事件字典
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
        //不确定目标是否存在，使用TryGetValue()
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
