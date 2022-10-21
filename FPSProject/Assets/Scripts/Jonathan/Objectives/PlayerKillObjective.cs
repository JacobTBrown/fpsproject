using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Scripts.Jonathan;
using UnityEngine;

public class PlayerKillObjective : Objective
{
    public GameObject player;
    private static int DEFAULTKILLSNEEDED = 20;
    private int killsNeeded = -1;
    private int kills = 0;

    public PlayerKillObjective(GameObject player)
    {
        this.player = player;
        this.killsNeeded = DEFAULTKILLSNEEDED;
    }

    public PlayerKillObjective(GameObject player,int killsNeeded)
    {
        this.player = player;
        this.killsNeeded = killsNeeded;
    }
    public void handleEvent(PlayerKillEvent evt){
     //   if(evt.GetType() == typeof(PlayerKillEvent)){
            Debug.Log("PlayerKill Objective Updated Kills =" +kills);
            Debug.Log(evt.player.GetComponent<PhotonView>().ViewID + " VS " + player.GetComponent<PhotonView>().ViewID);
              //          PlayerKillEvent e = (PlayerKillEvent)evt;
            if(evt.player == player){
                kills++;
            GameObject.Find("RoomManager").GetComponent<PlayerStatsPage>().totalKills++;
            }
            if(kills>=killsNeeded){
             eventCompleted();
            }
       // }
    }
    public void eventCompleted(){
            Debug.Log("Kill Event Completed for " + player );
            ObjectiveCompletedEvent evt = Events.objectiveCompletedEvent;
            evt.objective = this;
            EventManager.Broadcast(evt);
    }
}
