using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviourPunCallbacks, IOnPhotonViewPreNetDestroy
{
    RPC_Functions rpcFunc;
    PhotonView PV;
    [ContextMenu("Exit")]
    private void Awake()
    {
    }
    private void Start()
    {
        rpcFunc = GetComponentInParent<RPC_Functions>();
    }
    public void goToTitleMenu()
    {
        //SceneManager.UnloadSceneAsync("ColemanWeaponsAndPowerups");
       // Invoke("afterRPC", 1);
        //SaveAndDestroy();
        PV = GameObject.Find("Player(Clone)").GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            Debug.Log(PV.ViewID);
            //PV.RPC("ClearRPCs", RpcTarget.OthersBuffered, PhotonNetwork.LocalPlayer);
           // PhotonNetwork.RemoveRPCs(PhotonNetwork.LocalPlayer);
            //PhotonNetwork.CleanRpcBufferIfMine(PV);
            //  PhotonNetwork.Destroy(PV);
           // Destroy(PV.gameObject);
        }
        else
        {
        }
        //DisconnectPlayer();
        //Destroy(GameObject.Find("GameManager"));
     
        //PhotonHandler.
        //PhotonNetwork.LeaveRoom();
        
        PhotonNetwork.Disconnect();
            //PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
       // GameObject.FindObjectsOfTypeAll<PhotonView>();
        //Debug.Log("destroyed " + PhotonNetwork.LocalPlayer);
        //PhotonNetwork.LeaveRoom();

    }
    public void HardRestart()
    {
        System.Diagnostics.Process.Start(Application.dataPath.Replace("_Data", ".exe")); //new program
        Application.Quit();
    }

    public void DesktopQuit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
    public void afterRPC()
    {
        PhotonNetwork.LeaveRoom(); 
    }

    public void OnPreNetDestroy(PhotonView rootView)
    { //this gets called before we destroy the game obj
        Debug.Log("Pre-destruction call");
        return;
        //throw new System.NotImplementedException();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // PhotonNetwork.DestroyPlayerObjects(otherPlayer);
        //PhotonView PV = GameObject.Find("Player(Clone)").GetComponent<PhotonView>();
        //Debug.Log(PV);
            //PhotonNetwork.OpCleanActorRpcBuffer(otherPlayer.ActorNumber);

            //Debug.Log("OnPlayerLeftRoom: " + otherPlayer.ActorNumber);
        //PhotonNetwork.CleanRpcBufferIfMine(PV);
        //PhotonNetwork.Disconnect();
        //PhotonHandler is doing cleanup
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
        //PhotonNetwork.Disconnect();
        while (PhotonNetwork.InRoom)
        {
            Debug.Log("leaving room..");
            yield return null;
        }
   /*     while (PhotonNetwork.LevelLoadingProgress < 1)
        {
            Debug.Log("loading from exit btn");
            yield return null;
        }*/
        PhotonNetwork.LoadLevel(0);
    }
    public override void OnLeftRoom()
    {
        //PhotonNetwork.Disconnect();
        //SceneManager.LoadScene(0);
        //MenuManager.loadScene = true;
       // PhotonNetwork.Disconnect();
       // PhotonNetwork.LoadLevel(0);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
       
        PhotonNetwork.LoadLevel(0);
        Debug.Log("onDisconnected callbac for " + cause);
        //base.OnDisconnected(cause);

    }

    public void SaveAndDestroy()
    {
        GameObject player = GameObject.Find("Player(Clone)");
        //PhotonNetwork.LocalPlayer.
        GameObject roomManager = GameObject.Find("RoomManager");
        PlayerStatsPage pstats = roomManager.GetComponent<PlayerStatsPage>();
        PhotonView PV = roomManager.GetComponent<PhotonView>();
        pstats.SavePlayer();
        Destroy(roomManager);
        Destroy(PV);
       

        
    }
}
