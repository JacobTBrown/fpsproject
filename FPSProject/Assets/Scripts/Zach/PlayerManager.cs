using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.Scripts.Jonathan;
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

    public void RegisterPlayer(GameObject player)
    {
        /*
            Adds Players to an Player List
            If player IsMine Enable Player Controls and Cameras


        */

        Debug.Log("New Player Registered");
        if(player.GetComponent<PhotonView>().IsMine)
        {
            Debug.Log("TEST1");
           ((MonoBehaviour)player.GetComponent<PlayerMovement>()).enabled = true;
  //       ((MonoBehaviour)player.GetComponent<PlayerShoot>()).enabled = true;
           ((MonoBehaviour)player.GetComponent<PlayerSettings>()).enabled = true;
           player.transform.Find("Player Camera").gameObject.GetComponent<Camera>().enabled = true;
           player.transform.Find("Player Camera").gameObject.GetComponent<PlayerCameraMovement>().enabled = true;
        }
    }
}

//TODO:
//we should instantiate the player prefab with it's PhotonId, and use Find("Player" + Photon.Network.playerID).