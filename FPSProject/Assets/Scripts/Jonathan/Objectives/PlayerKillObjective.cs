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
        EventManager.AddListener<PlayerKillEvent>(OnPlayerDeath);
    }

    public PlayerKillObjective(GameObject player,int killsNeeded)
    {
        this.player = player;
        this.killsNeeded = killsNeeded;
    }
    public void handleEvent(PlayerKillEvent evt){
     //   if(evt.GetType() == typeof(PlayerKillEvent)){
<<<<<<< HEAD
            Debug.Log("PlayerKill Objective Updated Kills =" + kills);
            Debug.Log(evt.player.GetComponent<PhotonView>().ViewID + " VS " + player.GetComponent<PhotonView>().ViewID);
        if (evt.player.GetComponent<PhotonView>().ViewID != player.GetComponent<PhotonView>().ViewID)
        {
            
            Debug.Log("kills++" + " for " + player.GetComponent<PhotonView>().ViewID);
        }
        //          PlayerKillEvent e = (PlayerKillEvent)evt;
        if (evt.player == player){
=======
            Debug.Log("PlayerKill Objective Updated");
              //          PlayerKillEvent e = (PlayerKillEvent)evt;
            if(evt.killedPlayer == player){
>>>>>>> Jonathan
                kills++;
           // if (evt.player.GetComponent<PhotonView>().IsMine)
           // {
                //GameObject.Find("RoomManager").GetComponent<PlayerStatsPage>().SetKills();
                Debug.Log("kills++" + " for " + evt.player.GetComponent<PhotonView>().ViewID);
                //Debug.Log(" or kills++" + " for " + player.GetComponent<PhotonView>().ViewID);
          //  }
            }
            if(kills>=killsNeeded){
             eventCompleted();
            }
       // }
    }
    public void eventCompleted(){
            Debug.Log("Kill Event Completed for " + player );
            ObjectiveCompletedEvent evt = Events.ObjectiveCompletedEvent;
            evt.objective = this;
            EventManager.Broadcast(evt);
    }

    public void OnPlayerDeath(PlayerKillEvent evy)
    {

        Debug.Log("HEEEEEEEEEEEEEEEEEEEEEEEEEELLLLLLLLLO");
    }
}
