using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

using Unity.Scripts.Jonathan;

/*
    Goal of Script
        - Monitor All Player Kills
        - Order Players into a List Order by Highest Kills
        - If Player Hits a Max Kill Amount then BroadCast Completed Event
*/
public class FreeForAllKillObjective : MonoBehaviourPunCallbacks, Objective
{
    [SerializeField] public int killCutOff = 10;
    
 
    
    public override void OnPlayerPropertiesUpdate(Player target, Hashtable propties)
    {
        updateScore event1 = Events.Updating;
        EventManager.Broadcast(event1);
        if(propties.ContainsKey("Kills"))
        {
            if((int)target.CustomProperties["Kills"] >= killCutOff)
            {
                EventCompleted();
                UpdateUI();
            }
        }
    }

    private void EventCompleted()
    {
        ObjectiveCompletedEvent evt = Events.objectiveCompletedEvent;
        evt.objective = this;
        EventManager.Broadcast(evt);
    }
    void UpdateUI()
    {

    }

}
