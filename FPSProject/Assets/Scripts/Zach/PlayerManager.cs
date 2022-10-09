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

    SpawnManager spawnReference;
    PhotonView PV;
    [SerializeField] GameObject SettingsPanel;
    //added this for player spawn position - Jacob
    public GameObject playerPrefab;
    PlayerSettings playerSettings;
    public string playerstring = Path.Combine("PhotonPrefabs", "Player");
    GameObject canvas;
    Vector3 initSpawn = new Vector3(0, 5, 0);
   /*
   private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    */
    CharacterController cc;
    GameObject controllerAsGameObject;
    //GameObject controllerRefrence
    //CharacterController cc;
    private void Start()
    {
        /*
        spawnReference = GameObject.Find("GameManager").GetComponent<SpawnManager>();
        if (PV.IsMine)
        {
            createController();
        }
<<<<<<< HEAD
*/
=======
>>>>>>> Jacob
    }

    void createController()
    {
        // int seed = Random.Range(0, spawnReference.SpawnPoints.length);
        Debug.Log("PlayerManager.cs called createController()!");

        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), initSpawn, Quaternion.identity);
        //https://forum.photonengine.com/discussion/1577/how-to-use-photonnetwork-instantiate-with-object-data
        //TODO: WRITE MULTIPLAYER LOGIC FOR JONATHAN'S CODE
        //List<SpawnController>spawnPoint = SpawnManager.Instance.SpawnPoints;
        //with spawn points we need a reference to the player before we kill it. Because we literally destroy the GameObject
<<<<<<< HEAD
     //   controllerAsGameObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), initSpawn, Quaternion.identity, 0, new object[] { PV.ViewID });  
        controllerAsGameObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), initSpawn, Quaternion.identity, 0);
=======
        controllerAsGameObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), spawnReference.strategy.HandlePlayerSpawn(playerPrefab, spawnReference.SpawnPoints), Quaternion.identity, 0, new object[] { PV.ViewID });  
>>>>>>> Jacob
        //Instantiates the player prefab with a PhotonViewID
        //With that, the playerController will have a reference to send data back to the PlayerManager
    }

<<<<<<< HEAD
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
=======
>>>>>>> Jacob
    public void KillPlayer()
    {
        PhotonNetwork.Destroy(controllerAsGameObject);
        Debug.Log("PlayerManager.cs called KillPlayer()");
        //PhotonNetwork.Destroy(controllerReference);
        createController();
    }
}

//TODO:
//we should instantiate the player prefab with it's PhotonId, and use Find("Player" + Photon.Network.playerID).