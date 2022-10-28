using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Unity.Scripts.Jonathan;

using Hashtable = ExitGames.Client.Photon.Hashtable;

//Currently Unused, but its being created in each scene. I think 
//???
public class GameManager : MonoBehaviourPunCallbacks
{
    //public GameObject playerPrefab;
    //public GameObject flagPrefab;
    //public GameObject flagReturnPrefag;
    //public string flagPrefabString = "PhotonPrefabs/flag";
    //public string flagReturnPrefabString = "PhotonPrefabs/FlagReturn";
    //List<PhotonView> pvList = new List<PhotonView>();
    //list of players and their stats
    //kills of each player 

    //right now, I'm using PlayerManager instead.
    //Later, We'll want to instantiate other objects as prefabs that are not unique to a single player.
    //That's what this file is for.
    //I might do it elsewhere, not sure yet.

    #region Photon Callbacks


    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>

    GameMode gameMode;

    private void Start()
    {
        SetPlayerProperties();
        SetListeners();
        SetGameMode();

        Debug.Log("Creating GameManager");
    }
    public override void OnLeftRoom()
    {
        //Photon.Pun.SceneManager.LoadScene(0);
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        //introduces an extra roomManager
        //probably not going to use this file anyway, but if you do..
        //TODO: delete room manager

    }


    #endregion


    #region Public Methods


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


    #endregion

    private void SetPlayerProperties()
    {
        Hashtable PlayerCustomProps = new Hashtable();
        PlayerCustomProps["Kills"] = 0;
        PlayerCustomProps["Deaths"] = 0;
        PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCustomProps);
    }
    private void SetListeners()
    {
        EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);
        EventManager.AddListener<PlayerKillEvent>(OnPlayerKill);

    }

    public void OnPlayerDeath(PlayerDeathEvent evt)
    {
     //   #Needs to grab player from Event
       Player player = evt.player.GetComponent<PhotonView>().Controller;
        if(PhotonNetwork.IsMasterClient)
        {
            int Deaths = (int)player.CustomProperties["Deaths"];
            Deaths++;

            Debug.Log(player.NickName + "A Player Has Died. Deaths: " + Deaths);
            
            Hashtable PlayerCustomProps = new Hashtable();
            PlayerCustomProps["Deaths"] = Deaths;
            player.SetCustomProperties(PlayerCustomProps);
        }

    }

    public void OnPlayerKill(PlayerKillEvent evt)
    {
    //    #Needs to grab player from Event
       Player player = evt.player.GetComponent<PhotonView>().Controller;
        if(PhotonNetwork.IsMasterClient)
        {
            int Kills = (int)player.CustomProperties["Kills"];
            Kills++;
            Debug.Log(player.NickName + "Has Killed A Player. Kills: " + Kills);
            Hashtable PlayerCustomProps = new Hashtable();
            PlayerCustomProps["Kills"] = Kills;
            player.SetCustomProperties(PlayerCustomProps);
        }

    }
    private void SetGameMode()
    {
        string gameModeType = (string)PhotonNetwork.CurrentRoom.CustomProperties["GAMEMODE"]; 
       // if(gameModeType=="FFA")
      //  {
            int killCutOff = 20;
           gameMode = gameObject.AddComponent<FreeForAll>();
      //  }

        gameMode.CreateGameRules();
        gameMode.CreateGameObjectives();
        gameMode.LoadGameModeUI();
    }



}
/*
 * //replace with something like
// #Important
// used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
if (photonView.isMine)
{
    PlayerManager.LocalPlayerInstance = this.gameObject;
}
// #Critical
// we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
DontDestroyOnLoad(this.gameObject);
*/