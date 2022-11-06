using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

public class RoomListItemNew : MonoBehaviourPunCallbacks
{
    //[SerializeField] TMP_Text text;

    public RoomInfo info;
    public Text sizeText;
    public Text nameText;
    public Text mapText;
    public Text modeText;
    
    public static RoomListItemNew Instance;
    public static bool debug = false;
    //[SerializeField] public GameObject pref; // if i need a reference to the game object of the prefab


    public void Awake()
    {
        Instance = this;
    }
    /*
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
            else
            {
                Debug.Log(nameText.text + " was not equal to " + r.Name);
            }
            
        }
    */
    public void Setup(RoomInfo _info) //, Hashtable maps)
    { //is unused
        if (debug) Debug.Log("info from setup: " + _info);
            info = _info;
            sizeText.text = info.PlayerCount.ToString();
            sizeText.text += "/" + info.MaxPlayers; 
            //Debug.Log(info.Name + "was the name");
            nameText.text = info.Name;
            mapText.text = Launcher.Instance.mapsArr[(int)_info.CustomProperties["map"]-1].name;
         //modeText.text = Launcher.Instance.mapValue.text;
        //mapText.text = maps[info.CustomProperties["map"]].name;

    }
    public void OnClick(RoomInfo info)
    {
        if (debug) Debug.Log(info);
        if (this.info.MaxPlayers != this.info.PlayerCount)
        {
            //sizeText.text = (1 + info.PlayerCount).ToString(); only changes it for the person clicking the button
            Launcher.Instance.JoinRoom(info);
        } 
        else
        {
            //they are kicked to the title screen, but there is only a small (1 sec) window where a user can click the room before it is removed, so i'll just leave it like this.
        }
    }
}

