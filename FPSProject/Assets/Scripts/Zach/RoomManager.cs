
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO;
/*
    Author: Zach Emerson
    Creation: 9/19/22
    Last Edit: 9/30/22 -Zach

        RoomManager -> PlayerManager -> Player Controller
        The RooManager instantiates the PlayerManager, and the PlayerManager instantiates the player prefab.
        This is currently hard-coded to occur when we transition to scene one, OnSceneLoaded(1)
        
*/

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;
    private void Awake()
    {
        if (Instance)
        { //if another Instance of the RoomManager exists, delete and return
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
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
        if (scene.buildIndex == 1)
        {//instantiate the player prefab into scene 1 (ALL Players in the room execute this code in their own game)
            //Debug.Log("player prefab instantiate");
            //https://stackoverflow.com/questions/54981930/how-to-give-unique-ids-to-instantiated-objects-in-unity-c-sharp
                Vector3 initSpawn = new Vector3(0, 5, 0);
           PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), initSpawn, Quaternion.identity);  
        //Instantiates the player prefab with a PhotonViewID
            //https://forum.photonengine.com/discussion/1577/how-to-use-photonnetwork-instantiate-with-object-data
            //for (p in players[]){ PlayerManager.name = "player" + PhotonNetwork.NickName } //would require that everyone has a unique name. 
        }
    }

}

//to get the data from a gameObject that has a PhotonView component attatached: data = this.gameObject.GetPhotonView ().instantiationData;
//