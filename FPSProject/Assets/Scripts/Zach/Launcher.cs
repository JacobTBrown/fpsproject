using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;

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
    public int MaxPlayersPerLobby = 8;
    public int pingAsInt;
    public TMP_Text ping;
    //public GameObject Loadingpanel; //vs LoadingMenu 

    private void Awake()
    {
        Instance = this;
    }
    public void Update()
    {
        pingAsInt = PhotonNetwork.GetPing();
        if (pingAsInt == 0)
        {
            ping.text = "Bad Connection/Disconnected";
        }
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
        //Debug.Log("Nickname: " + PhotonNetwork.LocalPlayer.NickName, this);
        //bool customProperties = Player.SetCustomProperties(Hashtable t);
        Debug.Log("Current ping is " + PhotonNetwork.GetPing());
        
    }

    public override void OnJoinedLobby()
    {
        //Photon's defenition of 'Lobby': From the lobby, you can create a room or join a room
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("OnJoined Lobby Fucntion Call");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("Placeholder");
        Debug.Log("Nickname: " + PhotonNetwork.LocalPlayer.NickName, this);
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
            //Debug.Log("length of players array: " + players.Count());
            //Debug.Log("iteration: " + i);
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
        PhotonNetwork.LeaveRoom();
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
