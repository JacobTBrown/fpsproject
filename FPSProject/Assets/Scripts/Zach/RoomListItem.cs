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
    public PhotonView PV;
    [SerializeField] public GameObject pref;


    public void Awake()
    {
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("update room list from the prefab script");
        //Debug.Log(roomList.Count);
        //base.OnRoomListUpdate(roomList); 
        foreach (RoomInfo r in roomList)
        {
            
                Debug.Log("name:" + r.Name);
            Debug.Log("count:" + r.PlayerCount.ToString());
            if (nameText.text == r.Name)
            {
                sizeText.text = r.PlayerCount.ToString() + "/" + r.MaxPlayers;
            
                Debug.Log("put code here");
            
                if (r.CustomProperties.ContainsKey("map"))
                {
                    //if (r.CustomProperties.)
                    //sets all both rooms
                    Debug.Log("set map text");
                    // if (mapText.text == Launcher.Instance.)
                    mapText.text = Launcher.Instance.mapsArr[(int)r.CustomProperties["map"]].name;
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

