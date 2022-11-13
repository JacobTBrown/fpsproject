using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

using Unity.Scripts.Jonathan;
using TMPro;

/*
    Goal of Script
        - Monitor All Player Kills
        - Order Players into a List Order by Highest Kills
        - If a team Hits a Max Kill Amount then BroadCast Completed Event
        - copied by zach for teams
        - i prefer to use fewer files, sorry!
 */
public class TDMKillObjective : MonoBehaviourPunCallbacks, Objective
{
    [SerializeField] int killCutOff = 10;
    [SerializeField] GameObject GameOverText;
    public int team1Kills = 0;
    public int team2Kills = 0;
    public bool gameOver;
    public void Awake()
    {
        gameOver = false;
        team1Kills = 0;
        team2Kills = 0;
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if ((int)p.CustomProperties["team"] == 1)
            {
                team1Kills--;
            }
            if ((int)p.CustomProperties["team"] == 2)
            {
                team2Kills--;
            }
        }
    }
    public override void OnPlayerPropertiesUpdate(Player target, Hashtable propties)
    {
        if (gameOver)
        {
            return;
        }
        if (propties.ContainsKey("Kills"))
        {
            if ((int)target.CustomProperties["team"] == 1)
            {
               // Debug.Log("player on team 1 got kill: " + target.ActorNumber);
                team1Kills++;
            }
            else if ((int)target.CustomProperties["team"] == 2)
            {
             //   Debug.Log("player on team 2 got kill: " + target.ActorNumber);
                team2Kills++;
            }
            if (team1Kills >= killCutOff)
            {
                //do things for team 1
                EventCompleted();
               //UpdateUI(1);
            }
            else if (team2Kills >= killCutOff)
            {
                //do things for team 2
                EventCompleted();
                //UpdateUI(2);
            }
        }
    }

    private void EventCompleted()
    {
        gameOver = true;
        EndGameEvent evt = Events.EndGameEvent;
        ObjectiveCompletedEvent evt2 = Events.objectiveCompletedEvent;
        evt2.objective = this;
        EventManager.Broadcast(evt);
        EventManager.Broadcast(evt2);
    }
    void UpdateUI(int team)
    {   //is being handled by Jonathan now
        //GameOverText.transform.parent.gameObject.SetActive(true);
     //   GameOverText.GetComponent<TMP_Text>().text = "team " + team.ToString() + " wins with " + killCutOff + " kills!";
        // force-restart the game ? 
    } 

}
