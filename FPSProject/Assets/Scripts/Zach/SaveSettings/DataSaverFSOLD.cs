using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class DataSaverFSOLD
{
    private static bool debug = false;
    //Zach 10-10
    // PlayerStatsPage tells this file when to save data.
    // Data from PlayerStatsPage is serialized by DataToStore,
    // then the serialized data is saved to C:\Users\"UserName"\AppData\LocalLow\DefaultCompany\FPSProject\player.stats
    public static void SaveStatsFS(PlayerStatsPage playerStats)
    {

        if (debug) Debug.Log("saving");
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Path.Combine(Application.persistentDataPath + "player.stats");
        FileStream fs = new FileStream(path, FileMode.Create);
        if (debug) Debug.Log("path was " + path);
        DataToStore data = new DataToStore(playerStats);

        formatter.Serialize(fs, data);
        if (debug) Debug.Log("Save finished");
        fs.Close();
    }

    public static DataToStore LoadDataFS()
    {
        if (debug) Debug.Log("loading");
        string path = Path.Combine(Application.persistentDataPath + "player.stats");
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);

            formatter.Deserialize(fs);
            DataToStore data = formatter.Deserialize(fs) as DataToStore;
            fs.Close();
            if (debug) Debug.Log("load finished");
            return data;
        } catch (Exception e)
        {
            if (debug) Debug.Log("ERROR MESSAGE FROM UNITY: " + e);
            if (debug) Debug.Log("Path was: " + path);
            if (debug) Debug.Log("If this is your first time launching the game, you will have a new file on first auto-save");        
        }
        return null;
        /*if (File.Exists(path))
        {
                    BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);

            formatter.Deserialize(fs);
            DataToStore data = formatter.Deserialize(fs) as DataToStore;
            fs.Close();
            return data;
        }
        else
        {
        Debug.Log(" Error: file " + path + " not found -");
        }
        return data;*/
    }
}
