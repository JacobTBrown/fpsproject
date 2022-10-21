using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Scripts.Jonathan
{
    /* 
        Basic Event Engine taken from Unity FPS Tutorial

        - Edited by Jonathan Alexander on 9/27/2022 11:33pm
    */


    public class GameEvent
    {
    }

    // A simple Event System that can be used for remote systems communication
    public static class EventManager
    {
        static readonly Dictionary<Type, Action<GameEvent>> s_Events = new Dictionary<Type, Action<GameEvent>>();

        static readonly Dictionary<Delegate, Action<GameEvent>> s_EventLookups =
            new Dictionary<Delegate, Action<GameEvent>>();

        public static void AddListener<T>(Action<T> evt) where T : GameEvent
        {
            if (!s_EventLookups.ContainsKey(evt))
            {
                Action<GameEvent> newAction = (e) => evt((T) e);
                s_EventLookups[evt] = newAction;

                if (s_Events.TryGetValue(typeof(T), out Action<GameEvent> internalAction))
                    s_Events[typeof(T)] = internalAction += newAction;
                else
                    s_Events[typeof(T)] = newAction;
            }
        }

        public static void RemoveListener<T>(Action<T> evt) where T : GameEvent
        {
            if (s_EventLookups.TryGetValue(evt, out var action))
            {
                if (s_Events.TryGetValue(typeof(T), out var tempAction))
                {
                    tempAction -= action;
                    if (tempAction == null)
                        s_Events.Remove(typeof(T));
                    else
                        s_Events[typeof(T)] = tempAction;
                }

                s_EventLookups.Remove(evt);
            }
        }

        public static void Broadcast(GameEvent evt)
        {
            Debug.Log("broadcasting: " + evt);
            if (s_Events.TryGetValue(evt.GetType(), out var action))
            {
                Debug.Log("invoke: " + evt);
                action.Invoke(evt);
            }
        }

        public static void Clear()
        {
            s_Events.Clear();
            s_EventLookups.Clear();
        }
    }
}