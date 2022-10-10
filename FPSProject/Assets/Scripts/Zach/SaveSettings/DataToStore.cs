using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataToStore
{
    public float totalTime;
    public float timeInGame;
    public int kills;
    public int deaths;
    public Color playerColor;
    public double lastInterval;

    public DataToStore(PlayerStatsPage playerStats)
    {
        totalTime = playerStats.totalTime;
        timeInGame = playerStats.timeInGame;

        kills = playerStats.totalKills;
        deaths = playerStats.totalDeaths;
    }
}
