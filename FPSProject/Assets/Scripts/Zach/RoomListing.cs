using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RoomListing : MonoBehaviour
{
    [SerializeField] private Text _textName;
    //I wrote this to change room settings,
    //I did that in the launcher instead in createRoomOnClick
    public RoomInfo RoomInfo { get; private set; }
    
    public void SetRoomInfo(RoomInfo roomInfo)
    {
        Debug.Log("room info being set with " + roomInfo.Name + "and " + roomInfo.MaxPlayers);
        RoomInfo = roomInfo;
        _textName.text = roomInfo.MaxPlayers + "," + roomInfo.Name;
    }
   
}
