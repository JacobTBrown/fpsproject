using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviourPunCallbacks
{
    //[SerializeField] TMP_Text text;

    public RoomInfo info;
    public Text sizeText;
    public Text nameText;
    public Text mapText;
    //[SerializeField] public GameObject pref; // if i need a reference to the game object of the prefab


    public void Awake()
    {
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //Debug.Log("update room list from the prefab script");
        //Debug.Log(roomList.Count);
        //base.OnRoomListUpdate(roomList); 
        foreach (RoomInfo r in roomList)
        {
            //! for troubleshooting , 2:22am sunday 10/8

                 Debug.Log("name: " + r.Name);
                 Debug.Log("count: " + r.PlayerCount.ToString());
                if (nameText.text == r.Name)
                {
                    sizeText.text = r.PlayerCount.ToString() + "/" + r.MaxPlayers;

                    //   Debug.Log("put code here");

                    if (r.CustomProperties.ContainsKey("map"))
                    {
                        //sets all both rooms
                        //     Debug.Log("set map text");

                        mapText.text = Launcher.Instance.mapsArr[(int)r.CustomProperties["map"]].name; //for changing the map inside the room
                    }
                    else if (r.IsVisible)
                    {
                        Debug.Log("no map value in the hashtable was found");
                    }
                    else
                    {
                        Debug.Log("non-visible room has no hashtable");
                    }
                }
            
        }
    }
    public void Setup(RoomInfo _info) //, Hashtable maps) //!
    {
       
            info = _info;
            sizeText.text = info.PlayerCount.ToString();
            sizeText.text += "/" + info.MaxPlayers; 
            //Debug.Log(info.Name + "was the name");
            nameText.text = info.Name; //!
            mapText.text = Launcher.Instance.mapsArr[(int)_info.CustomProperties["map"]].name;
            
        //mapText.text = maps[info.CustomProperties["map"]].name;

    }

    public void OnClick()
    {
        if (info.MaxPlayers != info.PlayerCount)
        {
            //sizeText.text = (1 + info.PlayerCount).ToString();
            Launcher.Instance.JoinRoom(info);
        } 
        else
        {
            //play error sound
        }
    }
}

