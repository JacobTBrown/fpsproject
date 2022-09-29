using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    #region Photon Callbacks


    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>

    private void Start()
    {
        Debug.Log("Instantiating");
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 5, 0), Quaternion.identity);
    }
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