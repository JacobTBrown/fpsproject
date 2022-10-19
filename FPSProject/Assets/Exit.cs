using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviourPunCallbacks
{
    [ContextMenu("Exit")]
    public void goToTitleMenu()
    {
        //SceneManager.UnloadSceneAsync("ColemanWeaponsAndPowerups");

  
        //SaveAndDestroy();
        PhotonNetwork.LeaveRoom();
    }

    public void DisconnectPlayer()
    {
        StartCoroutine(DisconnectAndLoad());
    }
    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
        {
            Debug.Log("leaving room..");
            yield return null;
        }
        SceneManager.LoadScene(0);
    }
    public override void OnLeftRoom()
    {

        SceneManager.LoadScene(0);
        //MenuManager.loadScene = true;

    }

    public void SaveAndDestroy()
    {
        GameObject roomManager = GameObject.Find("RoomManager");
        PlayerStatsPage pstats = roomManager.GetComponent<PlayerStatsPage>();
        PhotonView PV = roomManager.GetComponent<PhotonView>();
        Destroy(roomManager);
        Destroy(PV);
        Debug.Log("room manager destroyed");
        pstats.SavePlayer();

        
    }
}
