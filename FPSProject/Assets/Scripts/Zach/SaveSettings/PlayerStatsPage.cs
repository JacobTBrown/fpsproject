using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Scripts.Jonathan;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;
// Zach 10-10: **MODEL** / VIEW / Controller
// Processes persistant stats for the player.
// This file dictates when to save - must exist in DoNotDestroyOnLoad to carry over into scenes.
// Also needs 
// 10-13  BUGFIX PLS: use onDestroy() to save!!! destroy it and save when we get back to scene 0!!
// 10-13 9pm: write a save & destroy function for the exit game button! ez fix
public class PlayerStatsPage : MonoBehaviour, IOnEventCallback
{

    /*    [HideInInspector] public float totalTime;
        [HideInInspector] public float timeInGame;
        [HideInInspector] public int totalKills;
        [HideInInspector] public int totalDeaths;*/
    [SerializeField] public float totalTime = 0;
    [SerializeField] public float timeInGame = 0;
    [SerializeField] public int totalKills = 0;
    [SerializeField] public int totalDeaths = 0;
    [SerializeField] public float updateInterval = 0.5f;
    [SerializeField] public double lastInterval;
    [SerializeField] GameObject KillPopUpPrefab;
 
    public int frames;
    public int level = 1;
    public int exp;
    public int newExp;
    public float fps;
    public static DataToStore data;
    public DataToStore newData;
    public static PlayerStatsPage Instance;
    [HideInInspector] public string json;
    bool debug = false;
    public bool inGame = false;
    float initialTimeInGame;
    public bool saved = false;
    public int gotKill;
    public int team1Kills;
    public int team2Kills;
    public int gotKilled;
    public bool onDie;
    public int team;
    PhotonView PPV;
    PhotonView EPV;
    private Transform content;

    private void Awake()
    {
        onDie = false; //unused with PhotonEvent.cs 
        EventManager.AddListener<PlayerKillEvent>(SetKills);
        EventManager.AddListener<PlayerDeathEvent>(SetDeaths);
        team1Kills = 0;
        team2Kills = 0;
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
        data = new DataToStore(this);
        newData = DataSaver.LoadData(data);
        loadNew();
        json = JsonUtility.ToJson(newData);
        if (debug) Debug.Log("Json was" + json);
        initialTimeInGame = timeInGame;
    }
    public void loadNew()
    {
        timeInGame = newData.timeInGame;
        totalTime = newData.totalTime;
        level = newData.level;
        exp = newData.exp; // exp is decresed, level in increased: level = exp % 120
        totalKills = newData.totalKills;
        totalDeaths = newData.totalDeaths;
        if (debug) Debug.Log("New data in playerstatspage: " + JsonUtility.ToJson(newData));
    }

    private void Start()
    {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
    }
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    public void SaveOnExit()
    {
        SavePlayer();
        saved = true;
        Destroy(gameObject);
    }
    private void Update()
    {
        ++frames;
        float timeNow = Time.realtimeSinceStartup;          //https://docs.unity3d.com/ScriptReference/Time-realtimeSinceStartup.html
        if (timeNow > lastInterval + updateInterval)
        {
            fps = (float)(frames / (timeNow - lastInterval));
            frames = 0;
            lastInterval = timeNow;
            totalTime += (float)updateInterval;
            if (inGame) timeInGame += (float)updateInterval;
            newExp = (int)(timeInGame - initialTimeInGame);  //only full integers are counted, no sense in tracking decimals here
        }
    }
    /// <summary>
    /// Returns true when we're on the same team, false otherwise. Call this before accepting damage from another player or anything else team related ?
    /// </summary>
    public bool CheckTeam(PhotonView EPV)
    {
        PhotonView myPV = GameObject.Find("Player(Clone)").GetComponent<PhotonView>();
        int myTeamNumber = (int)PhotonNetwork.LocalPlayer.CustomProperties["team"];
        if (myTeamNumber == 0)
        {
            return false;
        }
        Player enemyPlayer = null;
        if (myTeamNumber == 1 || myTeamNumber == 2) {
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (debug) Debug.Log("player was " + p.ActorNumber + " vs enemy player# " + EPV.OwnerActorNr);
                if (p.ActorNumber == EPV.OwnerActorNr)
                {
                    if (debug) Debug.Log("player was found: " + p.ActorNumber);

                    enemyPlayer = p;
                    if (debug) Debug.Log("PLayer teams: " + p.ActorNumber + " vs " + EPV.OwnerActorNr );
                }
            }
        } else { Debug.Log("team# =/0, 1, or 2"); }
        if (enemyPlayer == null)
        {
            if (debug) Debug.Log("player was not found in your room");
            return false;
        }
        int enemyTeamNumber = (int)enemyPlayer.CustomProperties["team"];
        if (enemyTeamNumber ==0)
        {
            if (debug) Debug.Log("!! Enemy player was in FFA!!");
        }
        else if (enemyTeamNumber == 1 && myTeamNumber == 1)
        {
                if (debug) Debug.Log("Both on team 1");
               return true;    
        }
        else if (enemyTeamNumber == 2 && myTeamNumber == 2)
        { 
                if (debug) Debug.Log("Both on team 2");
                return true;
        }
        if (debug) Debug.Log("CheckTeams() exited false, no condition met.");    
        return false;
    }
    public void SavePlayer()
    {
        if (exp > 0)
        {
            exp += (int)newExp;
            newExp = 0;
        }
        else
        {
            exp = (int)newExp;
            newExp = 0;
        }
        data = new DataToStore(this);
        //Debug.Log("in-game time: " + this.timeInGame);
        if (debug) Debug.Log("saving new data -- time: " + data.totalTime + " timeInGame: " + data.timeInGame + " exp " + data.exp + " level: " + data.level + " Kills: " + data.totalKills + " deaths: " + data.totalDeaths);
        if (debug) Debug.Log("initial time in game: " + initialTimeInGame + " new exp: " + newExp);
        //Debug.Log("saving " + JsonUtility.ToJson(data) +  "from PlayerStatsPage.cs");
        DataSaver.SaveStats(data);  
    }
    private void OnLevelWasLoaded(int level)
    {
        if (level > 0)
        {
            inGame = true;
            content = GameObject.Find("KillPopUp").transform;
        }
        else
        {
            inGame = false;
        }
    }
    public void LoadPlayer()
    {
        DataToStore newData = DataSaver.LoadData(data);
        totalTime = newData.totalTime;
        timeInGame = newData.timeInGame;
        level = newData.level;
        totalKills = newData.totalKills;
        totalDeaths = newData.totalDeaths;
    }
    private void OnDestroy()
    {
        if (!saved)
        {
            saved = true;
            SavePlayer();
        } 
        //GameObject.Destroy(gameObject);
    }
    public void StartInGameTimer()
    {
        
        if (SceneManager.GetActiveScene().buildIndex > 0){
            //if (debug) Debug.Log("logging");
            timeInGame += Time.timeSinceLevelLoad;
        }
    }
    public int GetTime()
    {
        //Debug.Log(timeInGame + "time from getTime");
        return (int)this.timeInGame;
    }
    public int GetTotalTime()
    {
        return (int)this.totalTime;
    }

    public int GetFPS()
    {
        return (int)this.fps;
    } 
    public int GetLevel()
    {
        return (int)this.level;
    }
     public int GetExp()
    {
        return (int)this.exp;
    }
    public int GetKills()
    {
        return this.totalKills;
    }
    public void SetKills()
    {
            totalKills++;
            return;
    }
    public void setDeaths()
    {
        totalDeaths++;
        return;
    }
    public void SetKills(PlayerKillEvent evt)
    { //unused with the PhotonEvents
    }
    public void SetDeaths(PlayerDeathEvent evt)
    {
    }
    public int GetDeaths()
    {
        return this.totalDeaths;
    }

    public void SetLevel(int lvl, int xp)
    {
        level = lvl;
        exp = xp;
        newExp = 0;
    }

   public void OnEvent(EventData photonEvent)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0)) 
        {
            //Debug.Log("dont do this on scene 0");
            return;
        }
        byte eventCode = photonEvent.Code;
        if (eventCode == 0) //PhotonEvents.PLAYERDEATH
        {
            object[] data = (object[])photonEvent.CustomData;
            int EnemyPlayer = (int)data[1]; //the photon view of the person who dealt damage

            EPV = PhotonNetwork.GetPhotonView(EnemyPlayer);
            PPV = PhotonNetwork.GetPhotonView((int)data[0]);
            KillPopUp(EPV, PPV);
            if (PPV.IsMine)
            {
                setDeaths();
            }
            else if (EPV.IsMine)
            {

                SetKills();
            }
        }
        else if (eventCode == PhotonEvents.JOINEDTEAM1)
        { //player player, int team
            object[] data = (object[])photonEvent.CustomData;
            team = (int)data[1];//the team of the player
        }   else if (eventCode == PhotonEvents.JOINEDTEAM2)
        {
            object[] data = (object[])photonEvent.CustomData;
            int EnemyPlayer = (int)data[1]; //the photon view of the person who dealt damage
        }
    }
    public void KillPopUp(PhotonView EPV, PhotonView PPV)
    {
        
        GameObject killPopUp = Instantiate(KillPopUpPrefab, content).gameObject;
        killPopUp.GetComponent<Text>().text = EPV.gameObject.GetComponent<PlayerSettings>().nickname + " killed " + PPV.gameObject.GetComponent<PlayerSettings>().nickname;
        StartCoroutine(fadeOut(killPopUp));
    }
    private IEnumerator fadeOut(GameObject bye)
    {
            yield return new WaitForSeconds(5f);
        Destroy(bye);
    }
}
