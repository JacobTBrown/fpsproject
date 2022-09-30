using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;
using ExitGames;

/*
    Author: Zach Emerson
    Creation: 9/2/22
    Last Edit: 9/29/22 -Zach

    Launcher.cs currently:
    -Auto connect to Photon on launch (its hard coded to only connect upon launching the game)
    -Displays ping
    -Allows for creation of Rooms
    -instantiates Prefabs for Players and Rooms in the 2D menu GUI
    -startGame() for the host of a Room
*/
public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] GameObject PlayerListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject startGameButton;
    [SerializeField] TMP_Text pingText;
    [SerializeField] Button[] multiplayerButtons;

    public int MaxPlayersPerLobby = 8;
    public int pingAsInt;
    //public bool isConnected;
    public TMP_Text ping;
    //public GameObject Loadingpanel; //vs LoadingMenu 

    private void Awake()
    {
        Instance = this;
        Invoke("CheckConnection", 30);
    }
    public void Update()
    {
        pingAsInt = PhotonNetwork.GetPing();
        ping.text = PhotonNetwork.GetPing().ToString();
        
    }
    void Start()
    {
        //Debug.Log("Attemting to connect");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;     
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    { //I think its deprecated but I can't find a replacement ?
        //throws error: Referenced Script is missing (from like 10 different callbacks)
   //     base.OnJoinRoomFailed(returnCode, message);
       Debug.Log("log error here");
    }
    public override void OnJoinedLobby()
    {
        //Photon's defenition of 'Lobby': From the lobby, you can create a room or join a room
        MenuManager.Instance.OpenMenu("welcome");
        //Debug.Log("OnJoined Lobby Fucntion Call");
        //PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("Placeholder");
        // Debug.Log("Nickname: " + PhotonNetwork.LocalPlayer.NickName, this);
    }
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            Debug.Log("Room name was null");
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("loading");
        //loading menu will automatically close after the async call above finishes executing
    }
   
    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("room");
        base.OnJoinedRoom();
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        //serverSettingsButton.SetActive(PhotonNetwork.IsMasterClient);
        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }
        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    //if the host leaves, another player is automatically given host privilages.
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed: " + message;
        //base.OnCreateRoomFailed(returnCode, message);
        MenuManager.Instance.OpenMenu("error");


    }
   // public void OnFailedToConnect(NetworkConnectionError error)
    //{
        //photon tells me to use this function but it doesn't work *shrugs* 
//    }
    public void CheckConnection()
    {
        if (pingAsInt == 0 || pingAsInt > 199) //When we're not connected, ping is 200
        {
            pingText.text = "Bad Connection/Disconnected";
            Debug.Log("bad connection");
            ConnectionFailed();
        }
    }
    public void ConnectionFailed()
    {
        //MenuManager.Instance.OpenMenu("title");
        Debug.Log("Connection Dropped, please try again or continue without connecting");
        errorText.text = "Connection Dropped, please try again or continue without connecting";
        MenuManager.Instance.OpenMenu("reconnect");
        for (int i = 0; i <multiplayerButtons.Length; i++)
        {
            multiplayerButtons[i].gameObject.SetActive(false);
        }

    }
    public void ConnectManually()
    {
        //needs better logic
        //its buggy 
        Debug.Log("attempting to reconnect...");
        PhotonNetwork.ConnectUsingSettings();
    }
    public void JoinRoom(RoomInfo info)
    {

        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");

    }
    public void JoinOnClick(RoomInfo info)
    {
        JoinRoom(info);
    }

    public void LeaveRoom()
    {
        //needs better logic
        
        PhotonNetwork.LeaveRoom(); //sends player to WelcomeScreen as a callback (The default state of Scene 0).
        //Finishes execution AFTER opening the title menu
        MenuManager.Instance.OpenMenu("title");

    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].PlayerCount >7) { roomList[i].RemovedFromList = true;  }
            if (roomList[i].RemovedFromList) { continue; }
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().Setup(roomList[i]);
        }
    }
    public void startGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);

    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
    }

}
