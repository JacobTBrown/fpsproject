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
    public void Setup(RoomInfo _info) //, Hashtable maps)
    { //is unused
        if (debug) Debug.Log("info from setup: " + _info);
            info = _info;
            sizeText.text = info.PlayerCount.ToString();
            sizeText.text += "/" + info.MaxPlayers; 
            //Debug.Log(info.Name + "was the name");
            nameText.text = info.Name;
            mapText.text = Launcher.Instance.mapsArr[(int)_info.CustomProperties["map"]-1].name;
    }
    public void OnClick(RoomInfo info)
    {
        if (debug) Debug.Log(info);
        if (this.info.MaxPlayers != this.info.PlayerCount)
        {

            Launcher.Instance.JoinRoom(info);
        } 
        else
        {
        }
    }
}

