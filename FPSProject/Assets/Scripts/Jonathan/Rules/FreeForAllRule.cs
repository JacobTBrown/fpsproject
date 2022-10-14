using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Scripts.Jonathan{

    /*
        Activates From RuleManager
        If a PlayerDeathEvent passes to handleEvent,
        Then Broadcast End Game Event.
    */
    public class FreeForAllRule : Rule
    {
        public void handleEvent(ObjectiveCompletedEvent evt)
        {      
            Debug.Log(evt.objective.GetType() +" VS " + typeof(PlayerKillObjective));
            if(evt.objective.GetType() == typeof(PlayerKillObjective))
            {
                Debug.Log("End Game Event Called");
                EndGameEvent ev = Events.EndGameEvent;
                EventManager.Broadcast(ev);
            }
        }
    }
}