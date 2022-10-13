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
   public static RoomListItemNew Instance;
    public bool debug = true;
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
    {
        if (debug) Debug.Log("info from setup: " + _info);
            info = _info;
            sizeText.text = info.PlayerCount.ToString();
            sizeText.text += "/" + info.MaxPlayers; 
            //Debug.Log(info.Name + "was the name");
            nameText.text = info.Name;
            mapText.text = Launcher.Instance.mapsArr[(int)_info.CustomProperties["map"]].name;
            
        //mapText.text = maps[info.CustomProperties["map"]].name;

    }
    
    /* launcher.cs style
    REPLACED WITH RENDERROOMS() 10-12 1:20PM      GameObject newRoomBtn = Instantiate(roomListItemPrefab, roomListContent) as GameObject;
        newRoomBtn.transform.Find("nameText").GetComponent<Text>().text = r.Name;
        newRoomBtn.transform.Find("sizeText").GetComponent<Text>().text = r.PlayerCount + "/" + r.MaxPlayers;
        if (r.CustomProperties.ContainsKey("map"))
        {
            //sets all both rooms
            //     Debug.Log("set map text");

            newRoomBtn.transform.Find("mapText").GetComponent<Text>().text = mapsArr[(int)r.CustomProperties["map"]].name; //for changing the map inside the room
        }
        newRoomBtn.GetComponent<Button>().onClick.AddListener(delegate { JoinOnClick(r); });*/
    public void OnClick(RoomInfo info)
    {
        Debug.Log(info);
        if (this.info.MaxPlayers != this.info.PlayerCount)
        {
            //sizeText.text = (1 + info.PlayerCount).ToString(); only changes it for the person clicking the button
            Launcher.Instance.JoinRoom(info);
        } 
        else
        {
            //play error sound
        }
    }
}

