using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// MVC (and *utils**
// MVC (and **utils**)
// This data is populated by PlayerStatsPage and then sent to the DataSaver
[Serializable]
public class DataToStore
{

    public float timeInGame;
    public float totalTime;
    public int level;
    public int exp;
    public int totalKills;
    public int totalDeaths;
    //public Color playerColor; to save a color, would need to write a system that transfers color values -> int or string -> back to color values or some named material/asset
    //public double lastInterval;

    public DataToStore(PlayerStatsPage playerStats)
    {
        totalTime = playerStats.totalTime;
        timeInGame = playerStats.timeInGame;
        level = playerStats.level;
        exp = playerStats.exp;
        totalKills = playerStats.totalKills;
        totalDeaths = playerStats.totalDeaths;
    }
    /// <summary>
    /// Get a new DataToStore object by adding the your current data object to a previous data object.
    /// <br>DataToStore(newData, OldData) </br> <br></br>Ex: DataToStore newData = new DataToStore(newData, oldData); 
    /// <br>timeInGame = newData.timeInGame + oldData.timeInGame</br>
    /// <br>exp = newData.exp</br>
    /// </summary>
    public DataToStore(DataToStore a, DataToStore b)
    {
        timeInGame = a.timeInGame + b.timeInGame;
        totalTime = a.totalTime + b.totalTime;
        level = a.level;
        exp = a.exp;
        totalKills = a.totalKills + b.totalKills;
        totalDeaths = a.totalDeaths + b.totalDeaths;
    }

   /* public static DataToStore operator +(DataToStore a, DataToStore b) ///OpeRaToR OvErLoADinG
     {
        DataToStore newData = newDataToStore(a, b);
        newData.
        return newData;
    }*/
}
