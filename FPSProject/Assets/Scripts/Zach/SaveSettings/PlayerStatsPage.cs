using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// Zach 10-10: **MODEL** / VIEW / Controller
// Processes persistant stats for the player.
// This file dictates when to save - must exist in DoNotDestroyOnLoad to carry over into scenes.
// Also needs 
// 10-13  BUGFIX PLS: use onDestroy() to save!!! destroy it and save when we get back to scene 0!!
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
    public float fps;
    public static DataToStore data;
    public DataToStore newData;
    public static PlayerStatsPage Instance;
    [HideInInspector] public string json;
    private void Awake()
    {
        // for if we need to persist it between scenes, we only keep the first one?
        //  if (Instance) 
        //   {
        //     Debug.Log("destroy");
        //   Destroy(this);
        //    }
        DontDestroyOnLoad(this.gameObject);
        //going to try making the JSON in DataSaver.cs
        Instance = this;
        //DataToStore data = new DataToStore(this);
        data = new DataToStore(this);
        //json = JsonUtility.ToJson(data);
        //Debug.Log("Json was" + json);
        newData = new DataToStore(data, DataSaver.LoadData(data));
        loadNew(); //invoke is not needed for json implementation
    }
    public void loadNew()
    {
        
        //Debug.Log("grabbing the new data loaded from PlayerStatsPage: " + data.totalKills);
        timeInGame = newData.timeInGame;
        totalTime = newData.totalTime;
        level = newData.level;
        exp = newData.exp;
        totalKills = newData.totalKills;
        totalDeaths = newData.totalDeaths;
        Debug.Log("New data in playerstatspage: " + JsonUtility.ToJson(newData));
    }
    private void Start()
    {
      //  if (Instance)
     //   {
       //     Debug.Log("destroy");
         //   Destroy(this);
    //    }
        
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
        

    }
    public void LogGameTime()
    {// call this OnDestroy()?

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
        }
    }
    public void SavePlayer()
    {
        //Save on destroy? zach 11:30 10-12
        data = new DataToStore(this);
        Debug.Log("saving data -- time: " + data.totalTime + " timeInGame: " + data.timeInGame + " level: " + data.level + " Kills: " + data.totalKills + " deaths: " + data.totalDeaths);
        //Debug.Log("saving " + JsonUtility.ToJson(data) +  "from PlayerStatsPage.cs");
        DataSaver.SaveStats(data);

        
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

    public void StartInGameTimer()
    {
        
        if (SceneManager.GetActiveScene().buildIndex > 0){
            Debug.Log("logging");
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
    public int GetDeaths()
    {
        return this.totalDeaths;
    }

    public void SetLevel(int l, int e)
    {
        level = l;
        exp = e;
    }
}
