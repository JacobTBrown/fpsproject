using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

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



    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Debug.Log("Attemting to connect");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        //Photon's defenition of 'Lobby': From the lobby, you can create a room or join a room.
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("OnJoined Lobby Fucntion Call");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("Placeholder");//doesn't work
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

        
        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }
        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Count(); i++)
        {   
            Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        } //re-instantiates players in the RoomMenu when someone leaves/joins
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        //serverSettingsButton.SetActive(PhotonNetwork.IsMasterClient);
        //if we want more options for the host
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
        //it says that it destroys objects for us when we leave
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList) { continue; }
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().Setup(roomList[i]);
            //populates the list of rooms in the FindRoomMenu
        }
    }
    public void startGame()
    {
        //REMOVED UNTIL 3D MULTIPLAYER WORKS - ZACH 
        //Loading Scene from MenuManager Instead
        //Clicking 'enter 3d lobby' from my Init scene will load the scene you have set to index 1
        PhotonNetwork.LoadLevel(1);
        //MenuManager.Instance.ChangeScene(1);
       
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
        //updates player list in the RoomMenu
    }
}
