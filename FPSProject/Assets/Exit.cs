using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviourPunCallbacks, IOnPhotonViewPreNetDestroy
{
    PhotonView PV;
    [ContextMenu("Exit")]
    private void Awake()
    {

    }
    public void goToTitleMenu()
    {
        //SceneManager.UnloadSceneAsync("ColemanWeaponsAndPowerups");

        
        //SaveAndDestroy();
        PV = GameObject.Find("Player(Clone)").GetComponent<PhotonView>();
        if (PV.IsMine)
        {

            //PhotonNetwork.CleanRpcBufferIfMine(PV);
            Debug.Log("exit destroyed u");
          //  PhotonNetwork.Destroy(PV);
        }
        else
        {
            Debug.Log("exit cant find ur PV");
        }
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
            //PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
       // GameObject.FindObjectsOfTypeAll<PhotonView>();
        //Debug.Log("destroyed " + PhotonNetwork.LocalPlayer);
        //PhotonNetwork.LeaveRoom();

    }

    public void OnPreNetDestroy(PhotonView rootView)
    { //this gets called before we destroy the game obj
        Debug.Log("Pre-destruction call");
        return;
        //throw new System.NotImplementedException();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //PhotonView PV = GameObject.Find("Player(Clone)").GetComponent<PhotonView>();

            PhotonNetwork.OpCleanActorRpcBuffer(otherPlayer.ActorNumber);
            Debug.Log("OnPlayerLeftRoom: " + otherPlayer.ActorNumber);
            //PhotonNetwork.CleanRpcBufferIfMine(PV);
            //PhotonNetwork.Destroy(PV);
 

       // Destroy()
    }
    public void DisconnectPlayer()
    {
        StartCoroutine(DisconnectAndLoad());
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
    }
    IEnumerator DisconnectAndLoad()
    {
       
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.InRoom)
        {
            Debug.Log("leaving room..");
            yield return null;
        }
        while (PhotonNetwork.LevelLoadingProgress < 1)
        {
            Debug.Log("loading from exit btn");
            yield return null;
        }
        PhotonNetwork.LoadLevel(0);
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
        //SceneManager.LoadScene(0);
        //MenuManager.loadScene = true;

    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene(0);
        Debug.Log("onDisconnected callbac for " + cause);
        base.OnDisconnected(cause);
    }

    public void SaveAndDestroy()
    {
        GameObject player = GameObject.Find("Player(Clone)");
        //PhotonNetwork.LocalPlayer.
        GameObject roomManager = GameObject.Find("RoomManager");
        PlayerStatsPage pstats = roomManager.GetComponent<PlayerStatsPage>();
        PhotonView PV = roomManager.GetComponent<PhotonView>();
        Destroy(roomManager);
        Destroy(PV);
        Debug.Log("room manager destroyed");
        pstats.SavePlayer();

        
    }
}
