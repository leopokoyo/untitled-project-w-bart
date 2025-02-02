using System;
using System.Collections.Generic;
using UnityEngine;

namespace _1_Scripts.Leo_s_Tools.GameManagers
{
    public class EventManager : MonoBehaviour
    {
        private Dictionary<string, Action<Dictionary<string, object>>> eventDictionary;

        private static EventManager eventManager;
        public static EventManager Instance
        {
            get
            {
                if (!eventManager)
                {
                    eventManager = FindFirstObjectByType<EventManager>();
                    if (!eventManager)
                    {
                        Debug.LogError("There is no EventManager in the scene!");
                    }
                    else
                    {
                        eventManager.Init();
                        DontDestroyOnLoad(eventManager);
                    }
                }

                return eventManager;
            }
        }


        private void Init()
        {
            eventDictionary ??= new Dictionary<string, Action<Dictionary<string, object>>>();
        }

        public static void StartListening(string eventName, Action<Dictionary<string, object>> listener)
        {
            if (Instance.eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent += listener;
                Instance.eventDictionary[eventName] = thisEvent;
            }
            else
            {
                thisEvent += listener;
                Instance.eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, Action<Dictionary<string, object>> listener)
        {
            if (eventManager == null) return;

            if (!Instance.eventDictionary.TryGetValue(eventName, out var thisEvent)) return;
            thisEvent -= listener;
            Instance.eventDictionary[eventName] = thisEvent;
        }

        public static void TriggerEvent(string eventName, Dictionary<string, object> eventData)
        {
            if (Instance.eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent?.Invoke(eventData);
            }
        } 
    }
}
