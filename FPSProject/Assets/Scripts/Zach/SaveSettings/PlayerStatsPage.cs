using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Scripts.Jonathan;
using UnityEngine;
using UnityEngine.SceneManagement;
// Zach 10-10: **MODEL** / VIEW / Controller
// Processes persistant stats for the player.
// This file dictates when to save - must exist in DoNotDestroyOnLoad to carry over into scenes.
// Also needs 
// 10-13  BUGFIX PLS: use onDestroy() to save!!! destroy it and save when we get back to scene 0!!
// 10-13 9pm: write a save & destroy function for the exit game button! ez fix
public class PlayerStatsPage : MonoBehaviour
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
    public int frames;
    public int level = 1;
    public int exp;
    public int newExp;
    public float fps;
    public static DataToStore data;
    public DataToStore newData;
    public static PlayerStatsPage Instance;
    [HideInInspector] public string json;
    bool debug = true;
    public bool inGame = false;
    float initialTimeInGame;
    public bool saved = false;
    private void Awake()
    {
        // for if we need to persist it between scenes, we only keep the first one?
        //  if (Instance) 
        //   {
        //     Debug.Log("destroy");
        //   Destroy(this);
        //    }

        EventManager.AddListener<PlayerKillEvent>(SetKills);

        DontDestroyOnLoad(this.gameObject);
        //going to try making the JSON in DataSaver.cs
        Instance = this;
        //DataToStore data = new DataToStore(this);
        data = new DataToStore(this);
        newData = DataSaver.LoadData(data);

        //invoke is not needed for json implementation
        loadNew();
        json = JsonUtility.ToJson(newData);
        if (debug) Debug.Log("Json was" + json);
        initialTimeInGame = timeInGame;
    }
    public void loadNew()
    {
        
        //Debug.Log("grabbing the new data loaded from PlayerStatsPage: " + data.totalKills);
        timeInGame = newData.timeInGame;
        totalTime = newData.totalTime;
        level = newData.level;
        exp = newData.exp; // exp is decresed, level in increased: level = exp % 500 probably?
        totalKills = newData.totalKills;
        totalDeaths = newData.totalDeaths;
        if (debug) Debug.Log("New data in playerstatspage: " + JsonUtility.ToJson(newData));
    }

    private void Start()
    {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
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
    public void SavePlayer()
    {
        if (exp > 0)
        {
            Debug.Log("saving with extra exp 0 exp: " + exp + "+= " +newExp);
            exp += (int)newExp;
            newExp = 0;
        }
        else
        {
            exp = (int)newExp;
            newExp = 0;
        }
       
        //Save on destroy? zach 11:30 10-12
        data = new DataToStore(this);

        //Debug.Log("in-game time: " + this.timeInGame);
        if (debug) Debug.Log("saving new data -- time: " + data.totalTime + " timeInGame: " + data.timeInGame + " exp " + data.exp + " level: " + data.level + " Kills: " + data.totalKills + " deaths: " + data.totalDeaths);
        if (debug) Debug.Log("initial time in game: " + initialTimeInGame + " new exp: " + newExp);
        //Debug.Log("saving " + JsonUtility.ToJson(data) +  "from PlayerStatsPage.cs");
        DataSaver.SaveStats(data);
        
    }
    private void OnLevelWasLoaded(int level)
    {
        if (debug) Debug.Log("scene transition to #" + level);
        if (level > 0)
        {
            inGame = true;
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
        //this.totalTime = Time.realtimeSinceStartup;
        if (debug) Debug.Log("Saving on exit" + this.totalTime);
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            if (debug) Debug.Log("logging");
            //timeInGame += Time.timeSinceLevelLoad;

        }
        if (!saved)
        {
            saved = true;
            SavePlayer();
        }
        
        //GameObject.Destroy(gameObject);
    }
    /*
    private void OnApplicationQuit()
    {
        this.totalTime = Time.realtimeSinceStartup;
        Debug.Log("Saving on exit" + this.totalTime);
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            Debug.Log("logging");
            timeInGame += Time.timeSinceLevelLoad;

        }
        SavePlayer();

    }
    */
    public void StartInGameTimer()
    {
        
        if (SceneManager.GetActiveScene().buildIndex > 0){
            if (debug) Debug.Log("logging");
            timeInGame += Time.timeSinceLevelLoad;
        }
        timeInGame += Time.timeSinceLevelLoad;
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
    public void SetKills(PlayerKillEvent evt)
    {
        //Debug.Log("PlayerStatsPage.cs Event: " + evt);
        totalKills++;
        //Debug.Log("total kills is now: " + totalKills);
        return;
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
}
