using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class RoomListItemOld : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    public RoomInfo info;
    public void Setup(RoomInfo _info)
    {
       /* it didnt work sadge
       if (PhotonNetwork.CurrentRoom != null)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 8)
            { return; }
        }*/
      
            info = _info;
            text.text = info.Name;
        
    }
    public void OnClick()
    {
        Launcher.Instance.JoinRoom(info);
    }
}

