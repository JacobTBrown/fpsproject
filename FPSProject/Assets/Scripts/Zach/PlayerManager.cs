using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.Scripts.Jonathan;
using Photon.Realtime;
using System.Linq;
/*
    Author: Zach Emerson
    Creation: 9/2/22
    Last Edit: 10/07/2022 - Jacob B

The class is attatched to a prefab
This class acts as the middle-man between the heigher-level RoomManager and the player controller (PlayerSettings.cs).
We are able to instantiate a player based on information given to ous from the RoomManager (ex: when a player dies, we will respawn them based on that data).
*/
public class PlayerManager : MonoBehaviour
{

     public List<PlayerController> players;
    PhotonView PV;
    PlayerSettings playerSettings;
    public string playerstring = Path.Combine("PhotonPrefabs", "Player");
    GameObject canvas;
    Vector3 initSpawn = new Vector3(0, 5, 0);
    CharacterController cc;
    GameObject controllerAsGameObject;
    //GameObject controllerRefrence
    //CharacterController cc;
    Vector3 randomSpawn;
    void Awake()
    {
        GameObject gamemgr = GameObject.Find("GameManager");
        EventManager.AddListener<PlayerDeathEvent>(onPlayerDeath);
        //EventManager.AddListener<PlayerSpawnEvent>(onPlayerSpawn);
        Debug.Log("Found Game Manager: " + gamemgr.name);
    }
    public void CreateNewPlayer()
    {
        Vector3 v = new Vector3(0,10,0);


        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), v, Quaternion.identity);
    }
    public void RegisterPlayer(GameObject player)
    {
        /*
            Adds Players to an Player List
            If player IsMine Enable Player Controls and Cameras


        */
        PlayerSpawnEvent s_evt = Events.PlayerSpawnEvent;
        NewPlayerEvent np_evnt = Events.NewPlayerEvent;
        np_evnt.player = player;
        s_evt.player = player;
        EventManager.Broadcast(s_evt);
        //Debug.Log("New Player Registered");
        EventManager.Broadcast(np_evnt);
        //Debug.Log(player.GetComponent<PhotonView>().ViewID + " Trying to register their player");
        if(player.GetComponent<PhotonView>().IsMine)
        {
           ((MonoBehaviour)player.GetComponent<PlayerMovement>()).enabled = true;
  //       ((MonoBehaviour)player.GetComponent<PlayerShoot>()).enabled = true;
           ((MonoBehaviour)player.GetComponent<PlayerSettings>()).enabled = true;
           player.transform.Find("Player Camera").gameObject.GetComponent<Camera>().enabled = true;
           player.transform.Find("Player Camera").gameObject.GetComponent<PlayerCameraMovement>().enabled = true;
            Debug.Log("Player finished camera setup: " + player.GetComponent<PhotonView>().ViewID);
            Debug.Log("Player's camera name is: " + player.transform.Find("Player Camera").gameObject.name);

           
        }
    }

    public void onPlayerDeath(PlayerDeathEvent evt){
        Debug.Log("Broadcasting " + evt + " from PlayerManager.cs");
        //edited 4pm 10-22
        PlayerSpawnEvent evt1 = Events.PlayerSpawnEvent;
        evt1.player = evt.player;
        EventManager.Broadcast(evt1);

    }   
    public void onPlayerSpawn(PlayerSpawnEvent evt){
        Debug.Log("Broadcasting " + evt + " from PlayerManager.cs");
        //edited 4pm 10-22
        PlayerSpawnEvent evt1 = Events.PlayerSpawnEvent;
        evt1.player = evt.player;
        EventManager.Broadcast(evt1);

    }

}

//TODO:
//we should instantiate the player prefab with it's PhotonId, and use Find("Player" + Photon.Network.playerID).