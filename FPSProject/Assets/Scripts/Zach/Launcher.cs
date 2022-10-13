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
using UnityEngine.SceneManagement;

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
    private Text roomPrefabName;
    private Text roomPrefabMap;
    private Text roomPrefabSize;
    [SerializeField] private RoomListItemNew _roomListItemPrefab;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [Header("In Room List")]
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] GameObject PlayerListItemPrefab;
    [SerializeField] Transform playerListContent;
    [Header("Host Options")]
    [SerializeField] GameObject startGameButton;
    [SerializeField] GameObject gameDMButton;
    [SerializeField] GameObject gameTDMButton;
    [SerializeField] GameObject mapSelectButton;
    [Header("Utils")]
    [SerializeField] TMP_Text errorText;
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
    private GameObject pingObj;
    private List<RoomInfo> AllRoomsList = new List<RoomInfo>();
    private List<GameObject> NewRoomsList = new List<GameObject>();
    public bool debug = true;
    private void Awake()
    {
        pingObj = GameObject.Find("PingVariable");
        pingObj.SetActive(false);
        //Debug.Log("Script activated");
        Instance = this;
        Invoke("CheckConnection", 30);
        mapsArr = new MapData[2];          //to add maps, increment this array, and add the map name below with its index.
        mapsArr[0] = new MapData("Map 1", 1);
        mapsArr[1] = new MapData("Map 2", 2);
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
        pingObj.SetActive(true);
        pingAsInt = PhotonNetwork.GetPing();
        ping.text = PhotonNetwork.GetPing().ToString();
        //Debug.Log("Connected");
        PhotonNetwork.JoinLobby(); //allows room list updates
        PhotonNetwork.AutomaticallySyncScene = true;     
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
       // Debug.Log("OnDisconnected() executed in launcher.cs");
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
        //Debug.Log("JoinedLobby.");
        if (!MenuManager.Instance.menus[1].open)
        {
            //Debug.Log("Opening Welcome Screen.");
            MenuManager.Instance.OpenMenu("welcome");
        }
        //MenuManager.Instance.OpenMenu("welcome");
        //Debug.Log("OnJoined Lobby Fucntion Call");
        // Debug.Log("Nickname: " + PhotonNetwork.LocalPlayer.NickName, this);
    }
    public void OnClickCreateRoom()
    {
        //https://answers.unity.com/questions/1718924/photon-network-wont-join-random-room-with-a-custom.html
        //ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
        //customProperties["Scene"] = selectedMap.map.name // this for the in-game properties i think?

        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            Debug.Log("Room name was null");
            return;
        }
        RoomOptions options = new RoomOptions();
        options.CustomRoomPropertiesForLobby = new string[] { "map" }; //!

        Hashtable properties = new Hashtable();             //custom properties with a hashtable- - 
        properties.Add("map", mapAsInt);                    //adds map name based on index in array above, index is changed by clicking button in CreateRoomMenu
        
        options.MaxPlayers = (byte)maxPlayersInput.value;   // - - default properties given by RoomOptions from Photon API
       
        //Debug.Log("You gave max players input: " + options.MaxPlayers);
        
        options.CustomRoomProperties = properties;

        PhotonNetwork.CreateRoom(roomNameInputField.text, options );
        //GameObject myRoomBtn = Instantiate(roomListItemPrefab, roomListContent) as GameObject;
        //string myText = myRoomBtn.transform.Find("nameText").GetComponent<Text>().text = roomNameInputField.text;
        //myRoomBtn.transform.Find("sizeText").GetComponent<Text>().text = "1/" + mapValue.text;

            //myRoomBtn.transform.Find("mapText").GetComponent<Text>().text = mapValue.text;
        //Debug.Log("created room with values" + myText);
    }
    private void ClearRoomList()
    {
        foreach (Transform t in roomListContent) //! fixes updates? 10-12 10pm
        {
            Destroy(t.gameObject);
        }
        //NewRoomsList.Clear();
    }
    public void RenderRooms()
    { //AllRoomsList has been updated, so store into NewRoomsList and render to screen
        ClearRoomList();    
        foreach (RoomInfo r in AllRoomsList)
        {
            if (debug)
                Debug.Log("rendering room : " + r.Name + "with info : " + r);
            GameObject newRoomItemPrefab = roomListItemPrefab;
            newRoomItemPrefab.GetComponent<RoomListItemNew>().Setup(r);
            newRoomItemPrefab.transform.Find("nameText").GetComponent<Text>().text = r.Name;
                newRoomItemPrefab.transform.Find("sizeText").GetComponent<Text>().text = r.PlayerCount + "/" + r.MaxPlayers;
            if (r.CustomProperties.ContainsKey("map"))
            {
                //if (debug) Debug.Log("had key");
                newRoomItemPrefab.transform.Find("mapText").GetComponent<Text>().text = mapsArr[(int)r.CustomProperties["map"]].name; //for changing the map inside the room
            }
            //RoomListItemNew roomListItem = newRoomItemPrefab.GetComponent<RoomListItemNew>();
            //RoomListItemNew roomListItem = newRoomItemPrefab.GetComponent<RoomListItemNew>();
            //roomListItem = newRoomItemPrefab.GetComponent<RoomListItemNew>();
            //roomListItem.Setup(r);

            if (debug) Debug.Log("info: " + newRoomItemPrefab.GetComponent<RoomListItemNew>().info);
                //roomListItem.GetComponent<Button>().onClick.AddListener(delegate { JoinOnClick(r); });
                //newRoomItemPrefab.GetComponent<Button>().onClick.AddListener(delegate { JoinOnClick(r); });
                //newRoomItemPrefab.transform.Find("mapText").GetComponent<Text>().text = r.Name;
                //Debug.Log("instantiating that: " + newRoomItemPrefab.name);
            GameObject newRoomBtn = Instantiate(roomListItemPrefab, roomListContent) as GameObject;
            newRoomBtn.GetComponent<Button>().onClick.AddListener(delegate { JoinOnClick(r); });
            //NewRoomsList.Add(newRoomBtn);
        }
        /*  foreach (GameObject newRoomBtn in NewRoomsList)
          {
              SetRoomInfo(newRoomBtn, AllRoomsList);
          }*/
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    { //this function creates the AllRoomsList. The AllRoomsList populates the newRoomList with RenderRooms() 
        //foreach (RoomInfo r in AllRoomsList) { Debug.Log("all names in AllRoomsList: " + r.Name); }
        foreach (RoomInfo r in roomList)
        {
            if (r.PlayerCount == 0 || r.PlayerCount == r.MaxPlayers)
            {
                if (debug) Debug.Log("removed 0 or hit count from room name: " + r.Name);
                 
                r.RemovedFromList = true;
                AllRoomsList.Remove(r);
                RenderRooms();
                continue;
            } else if (r.RemovedFromList){
                if (debug) Debug.Log("room was hidden or full");
                AllRoomsList.Remove(r);
                RenderRooms();
                continue;
            } 
            RoomInfo existingRoom = AllRoomsList.Find(x => x.Name.Equals(r.Name)); //foreach room, check to see it it already exists & store it in existingRoom
            if (existingRoom == null)
            {
                AllRoomsList.Add(r); //! check for proper removal of empty rooms, should have happened already: check RoomListingsMenu.cs
                if (debug) Debug.Log("added to all roomlist: " + AllRoomsList.Count);
            }
            else
            {//existing room was found, so update the info
                if (debug) Debug.Log("old player count: " + existingRoom.PlayerCount);
                if (debug) Debug.Log("new player count: " + r.PlayerCount);
                AllRoomsList.Remove(existingRoom);
                AllRoomsList.Add(r);
                

            }
            RenderRooms();
        }
        base.OnRoomListUpdate(roomList);
        //Debug.Log("new list of allRooms: " + roomList);
        foreach (RoomInfo r in AllRoomsList) { if (debug) Debug.Log("new names in AllRoomsList: " + r.Name); }
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
        //gameTDMButton.SetActive(PhotonNetwork.IsMasterClient);
        //gameDMButton.SetActive(PhotonNetwork.IsMasterClient);
        //mapSelectButton.SetActive(PhotonNetwork.IsMasterClient);
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
        //Debug.Log("created room" + roomNameText);

    }
    public void ChangeMap()
    {   //attatched to the select map button
        mapAsInt++;
        if (mapAsInt >= mapsArr.Length) mapAsInt = 0; //button click loops through the array
        //if (mapAsInt >= mapsArr.Length) mapsArr[mapAsInt].scene = 0;
        //Debug.Log("map int value: " + mapAsInt);
        //Debug.Log("map string value: " + mapsArr[mapAsInt].name);
        mapValue.text =  mapsArr[mapAsInt].name;
        
    }

    public void MaxPlayersSlider (float sliderInput)
    {
        //Debug.Log("Setting max players" + sliderInput);
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
            //pingText.text = "Connecting..";
            if (debug) Debug.Log("bad connection");
            ConnectionFailed();
        }
    }
    public void ConnectionFailed()
    {
        //MenuManager.Instance.OpenMenu("title");
        //Debug.Log("Connection Dropped, please try again or continue without connecting");
        errorText.text = "Connection Dropped, please try again or continue without connecting";
        MenuManager.Instance.OpenMenu("reconnect");
        for (int i = 0; i <multiplayerButtons.Length; i++)
        {
            multiplayerButtons[i].gameObject.SetActive(false);
        }
    }
    public void ConnectManually()
    {
        if (debug) Debug.Log("attempting to reconnect...");
        PhotonNetwork.ConnectUsingSettings();
    }
    public void JoinRoom(RoomInfo info)
    {
        if (debug) Debug.Log("This room has: " +info.MaxPlayers + "Max players"); 
        
        if (info.PlayerCount == info.MaxPlayers)
        {
            if (debug) Debug.Log("you tried to join a full room");
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
        //Debug.Log("called my LeaveRoom() handler");
        
        PhotonNetwork.LeaveRoom(); //sends player to WelcomeScreen as a callback (The default state of Scene 0).
        //Finishes execution AFTER opening the title menu
        
        
        //MenuManager.Instance.OpenMenu("title");

    }
    public void startGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
    public void startGameWithMap()
    {
        //Debug.Log("loading map number: " + mapsArr[mapAsInt].scene);
        PhotonNetwork.LoadLevel(mapsArr[mapAsInt].scene); //!
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    void SetRoomInfo(GameObject newRoomBtn, RoomInfo r)
    {
        //Debug.Log(newRoomBtn.transform.Find("nameText").GetComponent<Text>().text);
        Debug.Log(AllRoomsList.Count);
        RoomInfo existingRoom = AllRoomsList.Find(x => x.Name.Equals(newRoomBtn.transform.Find("nameText").GetComponent<Text>().text));
        if (existingRoom != null)
        { //room already existed, update count.
            newRoomBtn.transform.Find("sizeText").GetComponent<Text>().text = existingRoom.PlayerCount + "/" + existingRoom.MaxPlayers;
        }
        else
        {//room did not exist on lsit, so add it, then set name, count, map (other info later? like game type)
            AllRoomsList.Add(r);
            newRoomBtn.transform.Find("nameText").GetComponent<Text>().text = r.Name;
            if (existingRoom.CustomProperties.ContainsKey("map"))
            {
                newRoomBtn.transform.Find("mapText").GetComponent<Text>().text = mapsArr[(int)existingRoom.CustomProperties["map"]].name; //for changing the map inside the room
            }
            //newRoomBtn.GetComponent<Button>().onClick.AddListener(delegate { JoinOnClick(existingRoom); });
        }
        //newRoomBtn.transform.Find("nameText").GetComponent<Text>().text = AllRoomsList.Find
        /*   !! 2pm     newRoomBtn.transform.Find("sizeText").GetComponent<Text>().text = existingRoom.PlayerCount + "/" + existingRoom.MaxPlayers;
                if (existingRoom.CustomProperties.ContainsKey("map"))
                {

                    newRoomBtn.transform.Find("mapText").GetComponent<Text>().text = mapsArr[(int)existingRoom.CustomProperties["map"]].name; //for changing the map inside the room
                }
                newRoomBtn.GetComponent<Button>().onClick.AddListener(delegate { JoinOnClick(existingRoom); });*/
    }
}