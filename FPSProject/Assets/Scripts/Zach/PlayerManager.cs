﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.Scripts.Jonathan;
/*
    Author: Zach Emerson
    Creation: 9/2/22
    Last Edit: 9/30/22 -Zach

    The class is attatched to a prefab
    This class acts as the middle-man between the heigher-level RoomManager and the player controller (PlayerSettings.cs).
    We are able to instantiate a player based on information given to ous from the RoomManager (ex: when a player dies, we will respawn them based on that data).
 */
public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    [SerializeField] GameObject SettingsPanel;
    PlayerSettings playerSettings;
    GameObject canvas;
    Vector3 initSpawn = new Vector3(0, 5, 0);
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    
    CharacterController cc;

    //GameObject controllerRefrence
    //CharacterController cc;
    private void Start()
    {
        if (PV.IsMine)
        {
            createController();
        }

    }
    void createController()
    {
        

        //Debug.Log("PlayerManager.cs called createController()!");


        
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), initSpawn, Quaternion.identity);
        //https://forum.photonengine.com/discussion/1577/how-to-use-photonnetwork-instantiate-with-object-data
        //TODO: WRITE MULTIPLAYER LOGIC FOR JONATHAN'S CODE
        //List<SpawnController>spawnPoint = SpawnManager.Instance.SpawnPoints;
        //with spawn points we need a reference to the player before we kill it. Because we literally destroy the GameObject
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), spawnPoint.spawn[0], Quaternion.identity, 0, new object[] {//idk}) ;


    }
    public void KillPlayer()
    {
        Debug.Log("PlayerManager.cs called KillPlayer()");
        //PhotonNetwork.Destroy(controllerReference);
        createController();
    }
}

//TODO:
//we should instantiate the player prefab with it's PhotonId, and use Find("Player" + Photon.Network.playerID).