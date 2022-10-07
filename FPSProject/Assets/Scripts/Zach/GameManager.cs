using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    //public GameObject playerPrefab;
    //public GameObject flagPrefab;
    //public GameObject flagReturnPrefab;
    public string flagPrefabString = "PhotonPrefabs/Flag";
    public string flagReturnPrefabString = "PhotonPrefabs/FlagReturn";
    //right now, I'm using PlayerManager instead.
    //Later, We'll want to instantiate other objects as prefabs that are not unique to a single player.
    //That's what this file is for.
    //I might do it elsewhere, not sure yet.

    #region Photon Callbacks


    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>

    private void Awake()
    {
        Debug.Log("Instantiating New Objects");

        //PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 5, 0), Quaternion.identity);
        GameObject flagPrefabAsGameObj = PhotonNetwork.Instantiate(flagPrefabString, new Vector3(-9.62f, .88f, 27.35f), Quaternion.identity);
        GameObject flagReturnPrefabAsGameObj = PhotonNetwork.Instantiate(flagReturnPrefabString, new Vector3(-21.97f, 3.83f, 24.51f), Quaternion.identity);
    }
    /*private void Start()
    {
        Debug.Log("Instantiating New Objects");

        //PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 5, 0), Quaternion.identity);
        PhotonNetwork.Instantiate(flagPrefab.name, new Vector3(-9.62f, .88f, 27.35f), Quaternion.identity);
        PhotonNetwork.Instantiate(flagReturnPrefab.name, new Vector3(-21.97f, 3.83f, 24.51f), Quaternion.identity);
    }*/
    public override void OnLeftRoom()
    {
        //Photon.Pun.SceneManager.LoadScene(0);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);

    }


    #endregion


    #region Public Methods


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


    #endregion
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