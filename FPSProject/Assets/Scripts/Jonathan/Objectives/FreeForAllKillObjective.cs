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
    [SerializeField] public int killCutOff = 20;
    
    void Start()
    {
    
        EventManager.AddListener<updateScore>(OnPlayerScoreChange);
    }
    
    public void OnPlayerScoreChange(updateScore evt)
    {
        if(evt.propties.ContainsKey("Kills"))
        {
            if((int)evt.target.CustomProperties["Kills"] >= killCutOff)
            {
                EventCompleted();
            }
        }
    }

    private void EventCompleted()
    {
        ObjectiveCompletedEvent evt = Events.objectiveCompletedEvent;
        evt.objective = this;
        EventManager.Broadcast(evt);
    }

}
