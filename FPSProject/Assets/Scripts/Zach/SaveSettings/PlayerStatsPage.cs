using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// Zach 10-10: **MODEL** / VIEW / Controller
// Processes persistant stats for the player.
// This file dictates when to save - must exist in DoNotDestroyOnLoad to carry over into scenes.
// Also needs 
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
        Invoke("loadNew", 2); //it needs only one second to read/create the save file in my testing. 2 seconds is a safe value
        //loadNew(); probably needs an invoke, check back later //!
        /*        timeInGame = data.timeInGame;
                totalTime = data.totalTime;
                totalKills = data.totalKills;
                totalDeaths = data.totalDeaths;*/
        //  Debug.Log(data.totalTime);
    }
    public void loadNew()
    {
        
        //Debug.Log("grabbing the new data loaded from PlayerStatsPage: " + data.totalKills);
        timeInGame = newData.timeInGame;
        totalTime = newData.totalTime;
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
    {

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
        data = new DataToStore(this);
        Debug.Log("saving data -- time: " + data.totalTime + " timeInGame: " + data.timeInGame + " Kills: " + data.totalKills + " deaths: " + data.totalDeaths);
        //Debug.Log("saving " + JsonUtility.ToJson(data) +  "from PlayerStatsPage.cs");
        DataSaver.SaveStats(data);

        
    }

    public void LoadPlayer()
    {
        DataToStore newData = DataSaver.LoadData(data);
        totalTime = newData.totalTime;
        timeInGame = newData.timeInGame;
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
    public int getTime()
    {
        //Debug.Log(timeInGame + "time from getTime");
        return (int)this.timeInGame;
    }
    public int getTotalTime()
    {
        return (int)this.totalTime;
    }

    public int getFPS()
    {
        return (int)this.fps;
    }
    public int getKills()
    {
        return this.totalKills;
    }
    public int getDeaths()
    {
        return this.totalDeaths;
    }
}
