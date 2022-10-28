using System.Collections;
using System.Collections.Generic;
using Unity.Scripts.Jonathan;
using UnityEngine;

namespace Unity.Scripts.Jonathan{

    /*
        Activates From RuleManager
        If a PlayerDeathEvent passes to handleEvent,
        Then Broadcast End Game Event.
    */
    public class FreeForAllRule : MonoBehaviour
    {

        void Awake()
        {
            EventManager.AddListener<ObjectiveCompletedEvent>(onCompletedObjective);
        }

        public void onCompletedObjective(ObjectiveCompletedEvent evt)
        {      
           Debug.Log("Rule: Handle Event Called");
            if(evt.objective.GetType() == typeof(FreeForAllKillObjective))
            {
                Debug.Log("Rule: End Game Event Called");
                EndGameEvent ev = Events.EndGameEvent;
                EventManager.Broadcast(ev);
            }
        }
    }
}