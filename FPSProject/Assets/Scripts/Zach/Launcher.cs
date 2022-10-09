using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;
using ExitGames;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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
[System.Serializable]
public class MapData //!
{
    public string name;
    public int scene;
    public MapData(string n, int s)
    {
        this.name = n;
        this.scene = s;
    }
}
public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;
    [Header("Create")]
    [SerializeField] TMP_InputField roomNameInputField;
    //[SerializeField] public MapData[] mapsArr;
    [SerializeField] public TMP_Text mapValue;
    [SerializeField] public TMP_Text modeValue;
    [SerializeField] public Slider maxPlayersInput;
    [Header("Find Room List")]
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [Header("Room")]
    [SerializeField] GameObject PlayerListItemPrefab;
    [SerializeField] Transform playerListContent;
    [Header("Host Options")]
    [SerializeField] GameObject startGameButton;
    [SerializeField] GameObject gameDMButton;
    [SerializeField] GameObject gameTDMButton;
    
    [Header("Utils")]
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text pingText;
    [SerializeField] Button[] multiplayerButtons;
    public int currentMap = 0;

    public MapData[] mapsArr;
                //public string[] maps = { "test", "test2" };
    public int mapAsInt = 0;
    public int MaxPlayersPerLobby = 8;
    public int pingAsInt;
    public Text maxPlayersString;
    //public bool isConnected;
    public TMP_Text ping;
    //public GameObject Loadingpanel; //vs LoadingMenu 

    private void Awake()
    {
        //Debug.Log("Script activated");
        Instance = this;
        Invoke("CheckConnection", 30);
        mapsArr = new MapData[2];
        mapsArr[0] = new MapData("red", 1);
        mapsArr[1] = new MapData("blue", 2);
    }
    public void Update()
    {
        pingAsInt = PhotonNetwork.GetPing();
        ping.text = PhotonNetwork.GetPing().ToString();
        
    }
    void Start()
    {
        //PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
        //PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        if (!PhotonNetwork.IsConnected)
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");
        PhotonNetwork.JoinLobby(); //allows room list updates
        PhotonNetwork.AutomaticallySyncScene = true;     
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected() executed in launcher.cs");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    { //I think its deprecated but I can't find a replacement ?
        //throws error: Referenced Script is missing (from like 10 different callbacks)
   //     base.OnJoinRoomFailed(returnCode, message);
       //Debug.Log("Failed to join room, log error here" + message);
        MenuManager.Instance.OpenMenu("title");
 
    }
    public override void OnJoinedLobby()
    {
        //Photon's defenition of 'Lobby': From the lobby, you can create a room or join a room 
        if (!MenuManager.Instance.menus[1].open)
        MenuManager.Instance.OpenMenu("welcome");
       
        //Debug.Log("OnJoined Lobby Fucntion Call");
        //PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("Placeholder");
        // Debug.Log("Nickname: " + PhotonNetwork.LocalPlayer.NickName, this);
    }
    public void OnClickCreateRoom()
    {
        //https://answers.unity.com/questions/1718924/photon-network-wont-join-random-room-with-a-custom.html
        //ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
        //customProperties["Scene"] = selectedMap.map.name // this for the in-game properties i think?


        RoomOptions options = new RoomOptions();
        options.CustomRoomPropertiesForLobby = new string[] { "map" }; //!

        Hashtable properties = new Hashtable();
        properties.Add("map", mapAsInt);

        options.MaxPlayers = (byte)maxPlayersInput.value;
        //Debug.Log("You gave max players input: " + options.MaxPlayers);
        //options.CustomRoomPropertiesForLobby = new string[] { "Key" };
        options.CustomRoomProperties = properties;
        if (roomNameInputField != null)
        PhotonNetwork.CreateRoom(roomNameInputField.text, options );
        else
        {
            Debug.Log("null name");
        }
    }

    //REPLACED with ABOVE FUNCTION OnClickCreateRoom for room options fucntionality
   /* public void CreateRoom()
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
   
    */
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
        gameTDMButton.SetActive(PhotonNetwork.IsMasterClient);
        gameDMButton.SetActive(PhotonNetwork.IsMasterClient);
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
    public override void OnCreatedRoom()
    {
        Debug.Log("created room" + roomNameText);

    }
    public void ChangeMap()
    {   //attatched to the select map button
        mapAsInt++;
        if (mapAsInt >= mapsArr.Length) mapAsInt = 0; //button click loops through the array
        //if (mapAsInt >= mapsArr.Length) mapsArr[mapAsInt].scene = 0;
        Debug.Log("map int value: " + mapAsInt);
        Debug.Log("map string value: " + mapsArr[mapAsInt].name);
        mapValue.text = "Map: " + mapsArr[mapAsInt].name;
        
    }

    public void MaxPlayersSlider (float sliderInput) //!
    {
        Debug.Log("Seetting max players" + sliderInput);
        maxPlayersString.text = Mathf.RoundToInt(sliderInput).ToString();
    }
   public void ChangeGameMode()
    {

    }
    // public void OnFailedToConnect(NetworkConnectionError error)
    //{
    //photon tells me to use this function but it doesn't work *shrugs* 
    //    }
    public void CheckConnection()
    {
        if (pingAsInt < 21 || pingAsInt > 199) //When we're not connected, ping is 200
        {
            pingText.text = "Connecting..";
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
        Debug.Log("This room has: " +info.MaxPlayers + "Max players"); 
        
        if (info.PlayerCount == info.MaxPlayers)
        {
            Debug.Log("you tried to join a full room");
            return;
        }
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");

    }
    public void JoinOnClick(RoomInfo info)
    {
        JoinRoom(info);
    }

    public void LeaveRoom()
    {
        //PhotonNetwork.CurrentRoom.IsOpen = false;

        //needs better logic
        Debug.Log("called my LeaveRoom() handler");
        
        PhotonNetwork.LeaveRoom(); //sends player to WelcomeScreen as a callback (The default state of Scene 0).
        //Finishes execution AFTER opening the title menu
        

        MenuManager.Instance.OpenMenu("title");

    }
    //MOVED TO ROOMLISTINGSMENU.cs
   /* public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        
        for (int i = 0; i < roomList.Count; i++)
        {
            if (!roomList[i].IsVisible || !roomList[i].IsOpen || roomList[i].RemovedFromList)
            {
                Debug.Log("dont add to list");
                Destroy(roomList[i].trans)
            }
            else
            {
                Debug.Log(" add room to list: " + roomList[i].Name);
                Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().Setup(roomList[i]);
            }
            //if (roomList[i].PlayerCount >7) { roomList[i].RemovedFromList = true;  }
            //if (roomList[i].RemovedFromList) { continue; }
            
        }
    }*/
    public void startGame()
    {
        PhotonNetwork.LoadLevel(1); 
    }
    public void startGameWithMap()
    {
        Debug.Log("loading map number: " + mapsArr[mapAsInt].scene);
        PhotonNetwork.LoadLevel(mapsArr[mapAsInt].scene); //!
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);

    }
   

}

// we'll need a public variable to access spawn points
// programatically, each player should recieve a spawn point, (not necessarily different)
//   > we should be able to check for the most appropriate spawn based on some function such as
//		"checkForPlayerInRange()" function the that keeps track of other player's position - in GameManager.cs possibly? idk
//		not *quite* sure on the implementation quite yet, but we will need a static reference to this controller (probably in SpawnManager.cs or some Class that references the SpawnManager).
//		note* functions that are not attatched to an instantiated object/prefab be executed by all players, so this will need to be adapted a little for multiplayer.
//			** Instantiated objects are created at runtime, i.e., they will not exist in the heirarchy when the game is not executing.
//			so, the spawn points themselvs will not be instantiated, but we will need access to them to instantiate the players.
// -Zach
