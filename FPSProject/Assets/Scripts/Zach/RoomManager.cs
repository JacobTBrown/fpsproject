
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO;

using Unity.Scripts.Jonathan;
/*
    Author: Zach Emerson
    Creation: 9/19/22
    Last Edit: 9/30/22 -Zach
    MODEL / VIEW / **CONTROLLER**
        Room Manager exists in InitScene and is carried over into the game
        RoomManager -> PlayerManager -> Player Controller
        The RooManager instantiates the PlayerManager, and the PlayerManager instantiates the player prefab.
        This is currently hard-coded to occur when we transition to scene one, OnSceneLoaded(1)
        
*/

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;
    //added this for player spawn position - Jacob
    public GameObject playerPrefab;
    private SpawnManager spawnReference;
    private PlayerManager playerManager;
    public Vector3 initSpawn = new Vector3(0, 5, 0);
    private void Awake()
    {
        //list all players with their name
        if (Instance != null && Instance != this)
        { //if another Instance of the RoomManager exists, delete and return
          //Photon is actually throwing an error here. It doesn't keep the previous room manager. It instantly detects a duplicate PV and deletes the old one
            Destroy(gameObject);
            return;
        } else {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject); //RoomManager comes with us into the next scene so that we can instantiate the player.
    }
    public override void OnEnable()
    {
        base.OnEnable();
        //subscribes to unity's scene management class
        //when we change scenes, it will call OnSceneLoaded
        //Debug.Log("got to OnEnable()");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
   
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        //!
        if (scene.buildIndex >= 1) {
            playerManager = GameObject.Find("GameManager").GetComponent<PlayerManager>();
            //{//instantiate the player prefab into scene 1 (ALL Players in the room execute this code in their own game)
            Debug.Log("player prefab instantiate");
            //https://stackoverflow.com/questions/54981930/how-to-give-unique-ids-to-instantiated-objects-in-unity-c-sharp
            playerManager.CreateNewPlayer();
            PlayerStatsPage.Instance.StartInGameTimer();
            //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), initSpawn, Quaternion.identity); 
        }
         
        //Instantiates the player prefab with a PhotonViewID
            //https://forum.photonengine.com/discussion/1577/how-to-use-photonnetwork-instantiate-with-object-data
    }

}

//to get the data from a gameObject that has a PhotonView component attatached: data = this.gameObject.GetPhotonView ().instantiationData;
//