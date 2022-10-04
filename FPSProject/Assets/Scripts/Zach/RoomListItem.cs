using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    //[SerializeField] TMP_Text text;

    public RoomInfo info;
    [SerializeField] public TMP_Text text;

/*    public RoomInfo RoomInfo { get; set; }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        text.text = roomInfo.MaxPlayers + "," + roomInfo.Name;
    }*/
    public void Setup(RoomInfo _info)
    {
            info = _info;
            text.text = info.Name;
        
    }

    public void OnClick()
    {
        Launcher.Instance.JoinRoom(info);
    }
}

