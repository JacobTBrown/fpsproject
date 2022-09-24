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
    [SerializeField] GameObject serverSettingsButton;



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
        MenuManger.Instance.OpenMenu("title");
        //MenuManager.Instance.OpenMenu("title");
        Debug.Log("OnJoined Lobby Fucntion Call");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("Placeholder");
    }
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            Debug.Log("Room name was null");
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManger.Instance.OpenMenu("loading");
        //MenuManager.Instance.OpenMenu("loading");

    }
    public override void OnJoinedRoom()
    {
        MenuManger.Instance.OpenMenu("room");
        //MenuManager.Instance.OpenMenu("room");
        base.OnJoinedRoom();
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        serverSettingsButton.SetActive(PhotonNetwork.IsMasterClient);
        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }
        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Count(); i++)
        {
            Debug.Log("length of players array: " + players.Count());
            Debug.Log("iteration: " + i);
            Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }
    }

    //if the host leaves, another player is automatically given host privilages.
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        serverSettingsButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed: " + message;
        //base.OnCreateRoomFailed(returnCode, message);
        MenuManger.Instance.OpenMenu("error");
        //MenuManager.Instance.OpenMenu("error");


    }
    public void JoinRoom(RoomInfo info)
    {

        PhotonNetwork.JoinRoom(info.Name);
        MenuManger.Instance.OpenMenu("loading");
        //MenuManager.Instance.OpenMenu("loading");

    }
    public void JoinOnClick(RoomInfo info)
    {
        JoinRoom(info);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManger.Instance.OpenMenu("title");
        //MenuManager.Instance.OpenMenu("title");

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
}
