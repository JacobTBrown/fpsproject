using System.Collections;
using System.Collections.Generic;
using Unity.Scripts.Jonathan;
using UnityEngine;

public class TDMRule : Rule
{
    public void handleEvent(ObjectiveCompletedEvent evt)
    {
        Debug.Log(evt.objective.GetType() + " VS " + typeof(PlayerKillObjective));
        if (evt.objective.GetType() == typeof(PlayerKillObjective))
        {
            Debug.Log("End Game Event Called");
            EndGameEvent ev = Events.EndGameEvent;
            EventManager.Broadcast(ev);
        }
    }
}