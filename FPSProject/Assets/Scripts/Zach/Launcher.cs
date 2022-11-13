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
using System;
using Unity.Scripts.Jonathan;
using ExitGames.Client.Photon;

/*
    Author: Zach Emerson
    Creation: 9/2/22
    Last Edit: 9/29/22 -Zach

    Launcher.cs: Placed on the Canvas in InitialScene
    - on launch - Connects to server and calculates level based on play time
    - Implements Photon's API to: 
        --Create and Find rooms--Set properties of the room--Update the room when players join/leave--Launch the 3D game--
    - Photon provides callback functions to sync properties between clients.
    - Additional functions for animations and error messages
*/
[System.Serializable]

public class MapData //!
{
    public string name;
    public int scene;
    /// <summary>
    /// has fields for a name(string) and a scene(int).  
    /// MapData("name", *numberInBuildSettings*);
    /// </summary>
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
    [SerializeField] public TMP_Text mapValue;
    [SerializeField] public TMP_Text modeValue;
    [SerializeField] public Slider maxPlayersInput;
    [SerializeField] GameObject modeSelectButton;
    [SerializeField] RawImage MapImageCreateRoomRawImage;
    public int modeAsInt;
    [Header("Find Room List")]
    private Text roomPrefabName;
    private Text roomPrefabMap;
    private Text roomPrefabSize;
    [SerializeField] private RoomListItemNew _roomListItemPrefab;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [Header("In Room List")]
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] TMP_Text roomNameTeamText;
    [SerializeField] GameObject MapImage;
    [SerializeField] RawImage MapImageRawImage;
    [SerializeField] GameObject PlayerListItemPrefab;
    [SerializeField] GameObject PlayerListItemTeamsPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] Transform playerListTeam1;
    [SerializeField] Transform playerListTeam2;
    [Header("Host Options")]
    [SerializeField] GameObject startGameButton;
    [SerializeField] GameObject startGameTeamButton;
    [SerializeField] GameObject gameDMButton;
    [SerializeField] GameObject gameTDMButton;
    [SerializeField] GameObject mapSelectButton;
    [Header("Utils")]
    //[SerializeField] Image levelImage;
    [SerializeField] SpriteRenderer levelImage;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text errorText;
    [SerializeField] Button[] multiplayerButtons;
    [SerializeField] GameObject reconnectButton;
    [SerializeField] GameObject connectionFeedBackText;

    public int currentMap = 0;

    Hashtable playerProperties;
    
    public MapData[] mapsArr;
    private int mapAsInt = 1;
    public int MaxPlayersPerLobby = 8;
    public int pingAsInt;
    public Text maxPlayersString;
    //public bool isConnected;
    public TMP_Text ping;
    private GameObject pingObj;
    private List<RoomInfo> AllRoomsList = new List<RoomInfo>();
    public int exp;
    public int expTemp = 0;
    public int team1Size = 0;
    public int team2Size = 0;
    public bool debug;
    public bool playerAdded;
    private RoomInfo currentRoomInfo;
    float incrementSize = 500f;
    private void Awake()
    {
        MapImage.SetActive(false);
        debug = false;
        playerAdded = false;

        PhotonNetwork.OfflineMode = false;
        pingObj = GameObject.Find("Ping");
        pingObj.SetActive(false);
        Instance = this;
        Invoke("CheckConnection", 28);
        Invoke("LevelRoutine", 2);
        mapsArr = new MapData[3]; //to add a map, increment this array by one, and add the map name below where #=index in the build settings ex: (mapsArr[0] == 1)
        mapsArr[0] = new MapData("Ice World", 1); //give the map a name here, and insert the build index. The file name of the map and the map's image must match the naming scheme.
        mapsArr[1] = new MapData("Four Towers", 2);
        mapsArr[2] = new MapData("Grass Land", 3);
        modeAsInt = 0;
        playerProperties = new Hashtable();
        playerProperties.Add("Kills", 0);
        playerProperties.Add("Deaths", 0);
        playerProperties.Add("team", 0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }
    public void LevelRoutine()
    {
        int exp = (int)GameObject.Find("RoomManager").GetComponent<PlayerStatsPage>().newData.exp;
        int levelNumber = (int)GameObject.Find("RoomManager").GetComponent<PlayerStatsPage>().level;
        if (debug) Debug.Log("init exp: " + exp + " init level: " + levelNumber);
        LevelTracker(.03f, levelText, levelImage, levelNumber, exp);
    }
    private IEnumerator IntroFade()
    { //fade-in color
      
        if (GameObject.Find("WelcomeScreen").GetComponent<Image>() == null)
        {
            //Debug.Log("null");
            yield return new WaitForSeconds(.1f);
        }
        Image backgroundImg = GameObject.Find("WelcomeScreen").GetComponent<Image>();
        while (backgroundImg.color.a < 1.0f)
        {
            //Debug.Log("fade");
            backgroundImg.color = new Color(backgroundImg.color.r, backgroundImg.color.g, backgroundImg.color.b, backgroundImg.color.a + .005f);
            yield return new WaitForSeconds(.01f);
        }
    }
    private void LevelTracker(float time, TMP_Text levelText, SpriteRenderer img, int level, int exp)
    {
        int remainderExp = exp % 120;
        levelText.GetComponentInChildren<Text>().text = level.ToString();
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
        while (exp > 0)
        {
            // if (debug) Debug.Log(" exp % 120 is: " + exp % 120);
            if ((exp % 120) == 0)
            {
                levelText.GetComponentInChildren<Text>().text = (++level).ToString();
                //if (debug) Debug.Log("new level! : " + level);
            }
            img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a - .001f);
            exp--;
        }
        //if (debug) Debug.Log("setting exp: " + exp + " for level: " + level);
        GameObject.Find("RoomManager").GetComponent<PlayerStatsPage>().SetLevel(level, remainderExp);
        StartCoroutine(FadeOutLevelText(levelText));
    }
    private IEnumerator FadeOutLevelText(TMP_Text levelText)
    {
        while (levelText.color.a > 0.0f)
        {
            Color levelTextChildColor = levelText.GetComponentInChildren<Text>().color;
            //if (debug) Debug.Log("fade out");
            levelText.GetComponentInChildren<Text>().color = new Color(levelTextChildColor.r, levelTextChildColor.g, levelTextChildColor.b, levelTextChildColor.a - .01f);
            levelText.color = new Color(levelText.color.r, levelText.color.g, levelText.color.b, levelText.color.a - .01f);
            yield return new WaitForSeconds(.02f);
        }
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
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        pingObj.SetActive(true);
        pingAsInt = PhotonNetwork.GetPing();
        ping.text = PhotonNetwork.GetPing().ToString();
        if (!MenuManager.Instance.menus[1].open)
        {
            //Debug.Log("Opening Welcome Screen.");
            MenuManager.Instance.OpenMenu("welcome");
        }
        //Debug.Log("Connected");
        PhotonNetwork.JoinLobby(); //allows room list updates
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Invoke("CheckConnection", 5);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        gameObject.GetComponent<AudioSource>().Play();
        MenuManager.Instance.OpenMenu("title");
    }
    public override void OnJoinedLobby()
    {
        //Photon's defenition of 'Lobby': From the lobby, you can create a room or join a room 
        StartCoroutine(IntroFade());
    }
    public void OnClickCreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        if (modeAsInt > 0)
        {
            startGameTeamButton.SetActive(true);
        }
        else startGameButton.SetActive(true);
        RoomOptions options = new RoomOptions();
        options.CustomRoomPropertiesForLobby = new string[] { "map", "mode", "team1", "team2" }; //add more room properties here
        Hashtable properties = new Hashtable();       //RoomProperties with a hashtable- - 
        Hashtable playerProps = new Hashtable();     //PlayerProperties with a hashtable- - 
        properties.Add("map", mapAsInt);      //adds map name based on index in the mapArr, index is changed by clicking button in CreateRoomMenu
        properties.Add("mode", modeAsInt);
        if (modeAsInt == 0)
        {
            playerProps.Add("team", 0);
        }
        else if (modeAsInt == 1)
        {
            playerProps.Add("team", 1);
            properties.Add("team1", 0);
            properties.Add("team2", 0);
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
        options.MaxPlayers = (byte)maxPlayersInput.value;   // - - default properties given by RoomOptions from Photon API
        options.CustomRoomProperties = properties;
        PhotonNetwork.CreateRoom(roomNameInputField.text, options);
        DataManager.Instance.SetRoomName(roomNameInputField.text);
    }
    private void ClearRoomList()
    {
        foreach (Transform t in roomListContent) //! fixes updates? 10-12 10pm
        {
            Destroy(t.gameObject);
        }
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
                //if (debug) Debug.Log("had map key");
                newRoomItemPrefab.transform.Find("mapText").GetComponent<Text>().text = mapsArr[(int)r.CustomProperties["map"]-1].name; //for changing the map inside the room
            }
            if (r.CustomProperties.ContainsKey("mode"))
            {
                //if (debug) Debug.Log("had mode key");
                if ((int)r.CustomProperties["mode"] == 0)
                {
                    newRoomItemPrefab.transform.Find("modeText").GetComponent<Text>().text = "FFA   ";
                }else if ((int)r.CustomProperties["mode"] == 1)
                {
                    newRoomItemPrefab.transform.Find("modeText").GetComponent<Text>().text = "TDM ";
                }
                //if (debug) Debug.Log(r.CustomProperties["mode"].ToString());//for changing the map inside the room
            }
            if (debug) Debug.Log("info: " + newRoomItemPrefab.GetComponent<RoomListItemNew>().info);

            GameObject newRoomBtn = Instantiate(roomListItemPrefab, roomListContent) as GameObject;
            newRoomBtn.GetComponent<Button>().onClick.AddListener(delegate { JoinOnClick(r); });
        }
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    { //this function creates the AllRoomsList. When AllRoomsList contains all the rooms, RenderRooms() displays AllRoomsList
        //foreach (RoomInfo r in AllRoomsList) { Debug.Log("all names in AllRoomsList: " + r.Name); }
        foreach (RoomInfo r in roomList)
        {
            if (r.PlayerCount == 0 || r.PlayerCount == r.MaxPlayers)
            {
                r.RemovedFromList = true;
                AllRoomsList.Remove(r);
                RenderRooms();
                continue;
            }
            else if (r.RemovedFromList)
            { //catch all oth
                //if (debug) Debug.Log("room was hidden or full");
                AllRoomsList.Remove(r);
                RenderRooms();
                continue;
            }
            RoomInfo existingRoom = AllRoomsList.Find(x => x.Name.Equals(r.Name)); //foreach room, check to see it it already exists & store it in existingRoom
            if (existingRoom == null)
            { //
                AllRoomsList.Add(r);
                //if (debug) Debug.Log("added to all roomlist: " + AllRoomsList.Count);
            }
            else
            {//existing room was found, so update the info
                //if (debug) Debug.Log("old player count: " + existingRoom.PlayerCount);
                //if (debug) Debug.Log("new player count: " + r.PlayerCount);
                AllRoomsList.Remove(existingRoom);
                AllRoomsList.Add(r);
            }
            RenderRooms();
        }
        base.OnRoomListUpdate(roomList);
    }
    public void RenderPlayers(RoomInfo r)
    {
        Debug.Log("render with room info?");
        DataManager.Instance.SetRoomName(roomNameText.text);
        foreach (Transform child in playerListTeam1)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in playerListTeam2)
        {
            Destroy(child.gameObject);
        }
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0, j = 0; i < players.Count(); j++, i++)
        {
            if (debug) Debug.Log("rendering player: " + i + " on team: " + (int)players[i].CustomProperties["team"]);
            //players[i].CustomProperties["team"] = 1;
            if ((int)players[i].CustomProperties["team"] == 2)
            {
                // players[i].CustomProperties["team"] = 2;
                Instantiate(PlayerListItemTeamsPrefab, playerListTeam2).GetComponent<PlayerListItemTeam>().SetUp(players[i]);
            }
            else if ((int)players[i].CustomProperties["team"] == 1)
            {
                if (debug) Debug.Log(players[i].CustomProperties["team"].ToString());
                Instantiate(PlayerListItemTeamsPrefab, playerListTeam1).GetComponent<PlayerListItemTeam>().SetUp(players[i]);
            }
        }
        //if (debug) Debug.Log("rendered from RenderPlayers()");
    }
    public void RenderPlayers(Player[] p)
    {
        startGameTeamButton.SetActive(PhotonNetwork.IsMasterClient);
        Debug.Log("render with players array?");
        Player[] players;
        players = p;
        foreach (Transform child in playerListTeam1)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in playerListTeam2)
        {
            Destroy(child.gameObject);
        }
        Debug.Log(players.Count() + " total players");
        for (int i = 0, j = 0; i < players.Count(); j++, i++)
        {
            if (debug) Debug.Log("rendering player: " + i + " on team: " + (int)players[i].CustomProperties["team"]);
            if ((int)players[i].CustomProperties["team"] == 2)
            {
                Instantiate(PlayerListItemTeamsPrefab, playerListTeam2).GetComponent<PlayerListItemTeam>().SetUp(players[i]);
            }
            else if ((int)players[i].CustomProperties["team"] == 1)
            {
                if (debug) Debug.Log(players[i].CustomProperties["team"].ToString());
                Instantiate(PlayerListItemTeamsPrefab, playerListTeam1).GetComponent<PlayerListItemTeam>().SetUp(players[i]);
            }
        }
    }
    public void RenderPlayers()
    { //this function is either called from the OnPlayerPropertiesChanged() callback or Invoke() after 0.5sec
        MenuManager.Instance.OpenMenu("teamroom");
        //Debug.Log("RenderPlayers() with no parameter?");
        foreach (Transform child in playerListTeam1)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in playerListTeam2)
        {
            Destroy(child.gameObject);
        }
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0, j = 0; i < players.Count(); j++, i++)
        {
            if ((int)players[i].CustomProperties["team"] == 1)
            {
                Instantiate(PlayerListItemTeamsPrefab, playerListTeam1).GetComponent<PlayerListItemTeam>().SetUp(players[i]);
                continue;
            }
            if ((int)players[i].CustomProperties["team"] == 2)
            {
                Instantiate(PlayerListItemTeamsPrefab, playerListTeam2).GetComponent<PlayerListItemTeam>().SetUp(players[i]);
                continue;
            }
            if (debug) Debug.Log("not rendering player: " + i + " on team: " + (int)players[i].CustomProperties["team"]);
        }
        //if (debug) Debug.Log("rendered from RenderPlayers()");
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (debug) Debug.Log("player properties updated: " + changedProps.First());

        if ((int)PhotonNetwork.CurrentRoom.CustomProperties["mode"] > 0) 
        {
            //if (debug) Debug.Log("re-render");
            RenderPlayers();
        }
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
    }
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        //if (debug) Debug.Log("room properties update");
        if (propertiesThatChanged.ContainsKey("team1") && propertiesThatChanged.ContainsKey("team1"))
        {
            if (debug) Debug.Log(propertiesThatChanged["team1"] + " = team1 size, team2 size = " + propertiesThatChanged["team2"]);
        }
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        //if (PhotonNetwork.CurrentRoom)
        //team1RoomText.text = 
    }
    public override void OnJoinedRoom()
    {
        RoomInfo info = PhotonNetwork.CurrentRoom;
        if (debug) Debug.Log("room mode: " + info.CustomProperties["mode"].ToString());
        if (debug) Debug.Log("room map: " + info.CustomProperties["map"].ToString());
        if (info.CustomProperties.ContainsKey("map"))
        {
            if (debug) Debug.Log("set image to map" + (int)info.CustomProperties["map"] + "image");
            MapImageRawImage.texture = (Texture)Resources.Load("materials/map" + (int)info.CustomProperties["map"] + "image");
            MapImage.SetActive(true);
        } 
        MapImage.SetActive(true);
        
        base.OnJoinedRoom();
        if ((int)info.CustomProperties["mode"] > 0)
        {
            //if (debug) Debug.Log("room teams: " + info.CustomProperties["team1"].ToString() + " was team 1, team 2: " + info.CustomProperties["team1"].ToString());
            if (debug) Debug.Log("mode > 0");
            MenuManager.Instance.OpenMenu("teamroom");
            if ((int)info.CustomProperties["team2"] >= (int)info.CustomProperties["team1"])
            {
                //if (debug) Debug.Log("team 1 size: " + (int)info.CustomProperties["team1"] + "++");
                Hashtable h = new Hashtable();
                Hashtable j = new Hashtable();
                h.Add("team", 1);
                j.Add("team1" , 1 + (int)info.CustomProperties["team1"]);
                j.Add("team2" , (int)info.CustomProperties["team2"]);
                PhotonNetwork.CurrentRoom.SetCustomProperties(j);
                PhotonNetwork.LocalPlayer.SetCustomProperties(h);
            }
            else
            {
                Hashtable h = new Hashtable();
                Hashtable j = new Hashtable();
                h.Add("team", 2);
                j.Add("team1", (int)info.CustomProperties["team1"]);
                j.Add("team2", 1 + (int)info.CustomProperties["team2"]);
                PhotonNetwork.CurrentRoom.SetCustomProperties(j);
                PhotonNetwork.LocalPlayer.SetCustomProperties(h);
                //changeTeam(PhotonNetwork.LocalPlayer, 2);
                //if (debug) Debug.Log("new team 2 size: " + (int)info.CustomProperties["team2"]);
            }
            //Debug.Log("custom props rendering team: " + (int)PhotonNetwork.LocalPlayer.CustomProperties["team"] + " for player: " + PhotonNetwork.LocalPlayer.NickName);
            Invoke("RenderPlayers", 0.5f);
            //RenderPlayers();
        }

        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        roomNameTeamText.text = PhotonNetwork.CurrentRoom.Name;
        DataManager.Instance.SetRoomName(roomNameText.text);

        if (currentRoomInfo == null)
        { // currentRoomInfo is null when we create a room, only the first person runs this part.
            if (modeAsInt == 0) //is set when the host chooses the game mode
            {
                MenuManager.Instance.OpenMenu("room");
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
                return;
            }
            else if (modeAsInt == 1)
            {
                //if (debug) Debug.Log("onJoinedRoom my team # : " + PhotonNetwork.LocalPlayer.CustomProperties["team"].ToString());
                MenuManager.Instance.OpenMenu("teamroom");
                foreach (Transform child in playerListTeam1)
                {
                    Destroy(child.gameObject);
                }
                foreach (Transform child in playerListTeam2)
                {
                    Destroy(child.gameObject);
                }
                Player[] players = PhotonNetwork.PlayerList;

                for (int i = 0; i < players.Count(); i++)
                {
                    if ((int)players[i].CustomProperties["team"] == 1)
                    {
                        Instantiate(PlayerListItemTeamsPrefab, playerListTeam1).GetComponent<PlayerListItemTeam>().SetUp(players[i]);
                    }
                    else if ((int)players[i].CustomProperties["team"] == 2)
                    {
                        Instantiate(PlayerListItemTeamsPrefab, playerListTeam2).GetComponent<PlayerListItemTeam>().SetUp(players[i]);
                    }
   /*                 else if ((int)players[i].CustomProperties["team"] == 0)
                    {
                        if (debug) Debug.Log("your team was 0 instead of 1 or 2, not created " + players[i].CustomProperties["team"]);
                        //players[i].CustomProperties["team"] = (int)PhotonNetwork.CurrentRoom.CustomProperties["team2"] >= (int)PhotonNetwork.CurrentRoom.CustomProperties["team1"] ? 1 : 2;
                        //Instantiate(PlayerListItemTeamsPrefab, playerListTeam2).GetComponent<PlayerListItemTeam>().SetUp(players[i]);
                    }
                    // if (debug) Debug.Log("Team was " + (int)players[i].CustomProperties["team"]);*/
                }
                if (PhotonNetwork.IsMasterClient)
                {
                    startGameTeamButton.SetActive(PhotonNetwork.IsMasterClient);
                }
                    return;
            }
        }
        if (currentRoomInfo.CustomProperties.ContainsKey("mode"))
        {
            if ((int)currentRoomInfo.CustomProperties["mode"] == 0)
            {
                //if (debug) Debug.Log("mode zero");
                MenuManager.Instance.OpenMenu("room");
                foreach (Transform child in playerListContent)
                {
                    Destroy(child.gameObject);
                }
                Player[] players = PhotonNetwork.PlayerList;

                for (int i = 0; i < players.Count(); i++)
                {
                    Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
                }
            }
        }
        else
        {//no cases found, fallback code
            MenuManager.Instance.OpenMenu("room");
            foreach (Transform child in playerListContent)
            {
                Destroy(child.gameObject);
            }
            Player[] players = PhotonNetwork.PlayerList;

            for (int i = 0; i < players.Count(); i++)
            {
                Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
            }
        }
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public IEnumerator disableButtonBriefly()
    {
        startGameButton.SetActive(false);
        startGameTeamButton.SetActive(false);
        yield return new WaitForSeconds(1f);
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        startGameTeamButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if ((int)PhotonNetwork.CurrentRoom.CustomProperties["mode"] > 0)
        {
            startGameTeamButton.SetActive(PhotonNetwork.IsMasterClient);
        }
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("error");
    }
    public override void OnCreatedRoom()
    {
      // most stuff can be done in OnJoinedRoom() and simply check if we are the master client
    }
    public void ChangeMap()
    {   //attatched to the select map button
        mapAsInt++;
        if (mapAsInt >= mapsArr.Length+1) mapAsInt = 1; //button click loops through the array
        MapImageCreateRoomRawImage.texture = (Texture)Resources.Load("materials/map" + (mapAsInt).ToString() + "image");
        mapValue.text = mapsArr[mapAsInt-1].name;
    }

    public void MaxPlayersSlider(float sliderInput)
    {
        //Debug.Log("Setting max players" + sliderInput);
        maxPlayersString.text = Mathf.RoundToInt(sliderInput).ToString();
    }
    public void ChangeGameMode()
    {
        if (modeAsInt == 1)
        {
            modeAsInt = 0;
            modeValue.text = "Free For All";
        }
        else if (modeAsInt == 0)
        {
            modeAsInt = 1;
            modeValue.text = "Team Deathmatch";
        }
    }
    public void CheckConnection()
    {
        if (!PhotonNetwork.IsConnected)
        {
            gameObject.GetComponent<AudioSource>().Play();
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        for (int i = 0; i < multiplayerButtons.Length; i++)
        {
            multiplayerButtons[i].gameObject.SetActive(false);
        }
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.LocalPlayer.CustomProperties["team"] = 0;
        currentRoomInfo = null;
        MenuManager.Instance.OpenMenu("title");
        //base.OnLeftRoom();
    }
    public void ConnectManually()
    {
        connectionFeedBackText.SetActive(true);
        connectionFeedBackText.GetComponent<Text>().text = "Attempting to reconnect...";
        reconnectButton.SetActive(false);
        Debug.Log("reconn " + reconnectButton.name);
        if (debug) Debug.Log("attempting to reconnect...");
        PhotonNetwork.ConnectUsingSettings();
        StartCoroutine(connectionCheck(reconnectButton));
    }
    private IEnumerator connectionCheck(GameObject reconnectButton)
    {

        if (!PhotonNetwork.IsConnectedAndReady)
        {
            yield return new WaitForSeconds(10f);
        }
       reconnectButton.SetActive(true);
       // connectionFeedBackText.SetActive(false);
    }
    public void JoinRoom(RoomInfo info)
    {
        if (debug) Debug.Log("This room has: " + info.MaxPlayers + "Max players");

        if (info.PlayerCount == info.MaxPlayers)
        {
            errorText.text = "That room is full!";
            MenuManager.Instance.OpenMenu("error");
            if (debug) Debug.Log("you tried to join a full room");
            return;
        }
        if ((int)info.CustomProperties["mode"] > 0)
        {
            startGameTeamButton.SetActive(false);
        }
        //if (lowerTeam == 1) { PhotonNetwork.LocalPlayer.CustomProperties["team"] = 1; } else if (lowerTeam == 2) { PhotonNetwork.LocalPlayer.CustomProperties["team"] = 2; } else PhotonNetwork.LocalPlayer.CustomProperties["team"] = 0;
        PhotonNetwork.JoinRoom(info.Name);
        DataManager.Instance.SetRoomName(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }
    public void JoinOnClick(RoomInfo info)
    {
        currentRoomInfo = info;
        JoinRoom(info);
    }

    public void LeaveRoom()
    {
        MapImage.SetActive(false);
        PhotonNetwork.LeaveRoom(); //sends player to WelcomeScreen as a callback (The default state of Scene 0).
    }
    public void ChangeTeamButtonClick()
    {
        int currentTeam = (int)PhotonNetwork.LocalPlayer.CustomProperties["team"];
        //int newTeam;
        RoomInfo info = PhotonNetwork.CurrentRoom;
        Hashtable player = new Hashtable();
        Hashtable newRoomInfo = new Hashtable();
        if (currentTeam == 1)
        {
           // newTeam = 2;
            if (debug) Debug.Log("swap to team 2");
            newRoomInfo.Add("team1", (int)info.CustomProperties["team1"] -1);
            newRoomInfo.Add("team2", (int)info.CustomProperties["team2"] + 1);
            player.Add("team", 2);
        }
        else if (currentTeam == 2)
        {
            if (debug) Debug.Log("swap to team 1");
          //  newTeam = 1;
            player.Add("team", 1);
            newRoomInfo.Add("team2", (int)info.CustomProperties["team2"] - 1);
            newRoomInfo.Add("team1", 1 + (int)info.CustomProperties["team1"]);
        }
        else
        {
            if (debug) Debug.Log("no team found");
        }
        PhotonNetwork.CurrentRoom.SetCustomProperties(newRoomInfo);
        PhotonNetwork.LocalPlayer.SetCustomProperties(player);
    }
    public void startGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
    public void startGameWithMap()
    {
        GameObject.Find("Back").SetActive(false);
        //Debug.Log("loading map number: " + mapsArr[mapAsInt].scene);
        PhotonNetwork.LoadLevel(mapsArr[mapAsInt-1].scene); 
    }    
    public void startGameAndRemove()
    { // kicks you to title screen if you can click fast enough before it disappears
        GameObject.Find("Back").SetActive(false);
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.CurrentRoom.IsOpen = false;      
        PhotonNetwork.LoadLevel(mapsArr[mapAsInt-1].scene);
    }
    public void RenderFFA()
    {
            MenuManager.Instance.OpenMenu("room");
        if (debug) Debug.Log("RenderFFA()");
            foreach (Transform child in playerListContent)
            {
                Destroy(child.gameObject);
            }
            Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0, j = 0; i < players.Count(); j++, i++)
        {
            //if (roomNameTeamText.text == "2")
            if ((int)players[i].CustomProperties["team"] == 0)
            {
                Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
            }
            else Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
            //if (debug) Debug.Log(" rendering player: " + i + " on ffa with team: " + (int)players[i].CustomProperties["team"]);
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (debug) Debug.Log("player entered room ()");
        StartCoroutine(disableButtonBriefly());
        if ((int)PhotonNetwork.CurrentRoom.CustomProperties["mode"] > 0){
            Invoke("RenderPlayers", 0.5f);
        }
        else if ((int)PhotonNetwork.CurrentRoom.CustomProperties["mode"] == 0)
        {
            {
                if (debug) Debug.Log("player added to FFA room, mode  # was: " + (int)newPlayer.CustomProperties["team"]);
                // Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
                Invoke("RenderFFA", 0.5f);
            }
        }
            //int gameMode;
            if (PhotonNetwork.MasterClient == PhotonNetwork.LocalPlayer)
        {
            if (modeAsInt == 0)
            {
                if (debug) Debug.Log("dont do in ffa");
            }
            else
            {
                int team1 = 0;
                int team2 = 0;
                Player[] players = PhotonNetwork.PlayerList;
                foreach (Player p in players)
                {
                    if (p == newPlayer)
                    {
                        continue;
                    }
                    if ((int)p.CustomProperties["team"] == 1)
                    {
                        team1++;
                    }
                    else if ((int)p.CustomProperties["team"] == 2)
                    {
                        team2++;
                    }
                }
            }
        }
                if (debug) Debug.Log("new player entered room with team: " + (int)newPlayer.CustomProperties["team"]);
    }
    public void changeTeam(Player p, int team)
    {
        if (team >= 1)
        {
            RaiseEventOptions o = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            object[] obj = { p, team };
            PhotonNetwork.RaiseEvent(PhotonEvents.JOINEDTEAM1, obj, o, SendOptions.SendReliable);
        }
    }
}