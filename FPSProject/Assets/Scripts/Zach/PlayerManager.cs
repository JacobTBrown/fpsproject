using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.Scripts.Jonathan;

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

        //TODO: WRITE LOGIC FOR JONATHAN'S CODE
        //List<SpawnController>spawnPoint = SpawnManager.Instance.SpawnPoints;
        //with spawn points we need a reference to the player before we kill it. Because we literally destroy the GameObject
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), spawnPoint.spawn[0], Quaternion.identity, 0, new object[] {}) ;


    }
    public void KillPlayer()
    {
        Debug.Log("PlayerManager.cs called KillPlayer()");
        //PhotonNetwork.Destroy(controllerReference);
        createController();
    }
}
