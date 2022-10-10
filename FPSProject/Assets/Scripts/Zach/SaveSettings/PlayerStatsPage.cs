using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsPage : MonoBehaviour
{
    public float totalTime = 10f;
    public float timeInGame = 10f;
    public int totalKills = 1;
    public int totalDeaths = 1;

    public double lastInterval;
    public int frames;
    private float fps;

    private void Start()
    {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
    }
    private void OnGUI()
    {
        
    }
    private void Update()
    {
        
        ++frames;
    }
    public void SavePlayer()
    {
        DataSaver.SaveStats(this);

        
    }

    public void LoadPlayer()
    {
        DataToStore data = DataSaver.LoadData();
        totalTime = data.totalTime;
        timeInGame = data.timeInGame;
        totalKills = data.kills;
        totalDeaths = data.deaths;
    }
}
