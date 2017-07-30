using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameEvent
{
    EnterBridge,
    ExitBridge,
    EnterWarningArea,
    ExitWarningArea
};

public class EventManager : MonoBehaviour {

    private static EventManager Instance;
    private Dictionary<GameEvent, UnityEvent> eventDictionary;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = FindObjectOfType<EventManager>();

            if(!Instance)
            {
                // disabling because error message pops up when stop the game
                Debug.LogError("No object with an event manager found in the scene");
                Destroy(this.gameObject);
            }
            else
            {
                Instance.Init();
            }
         }
    }
    
    void Init()
    {
        if(eventDictionary == null)
        {
            eventDictionary = new Dictionary<GameEvent, UnityEvent>();
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public static void StartListening(GameEvent eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;

        if(!Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent = new UnityEvent();
            Instance.eventDictionary.Add(eventName, thisEvent);
        }

        thisEvent.AddListener(listener);
    }

    public static void StopListening(GameEvent eventName, UnityAction listener)
    {
        if (!Instance) return;

        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(GameEvent eventName)
    {
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}
