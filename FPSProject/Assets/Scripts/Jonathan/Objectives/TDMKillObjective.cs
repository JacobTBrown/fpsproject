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
        - copied by zach for teams
 */
public class TDMKillObjective : MonoBehaviourPunCallbacks, Objective
{
    [SerializeField] public int killCutOff = 10;
    public int team1Kills = 0;
    public int team2Kills = 0;
 
    
    public override void OnPlayerPropertiesUpdate(Player target, Hashtable propties)
    {
        if(propties.ContainsKey("Kills"))
        {
            
            if ((int)target.CustomProperties["team"] == 1)
            {
                Debug.Log("player on team 1 got kill: " + target.ActorNumber);
                team1Kills++;
            }
            else if ((int)target.CustomProperties["team"] == 2)
            {
                Debug.Log("player on team 2 got kill: " + target.ActorNumber);
                team2Kills++;
            }
            if (team1Kills >= killCutOff)
            {
                //do things for team 1
                EventCompleted();
                UpdateUI();
            }
            else if (team2Kills >= killCutOff)
            {
                //do things for team 2
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
