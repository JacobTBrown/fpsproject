using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class PhotonNetworkHandler : MonoBehaviourPunCallbacks
{
    public static PhotonNetworkHandler Instance;

    private void Awake()
    {
        Instance = this;
    }



    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("title");
        GameObject.FindGameObjectWithTag("LoadingMenu").SetActive(false);       

    }
   

}
