using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
public static class DataSaver
{

    public static void SaveStats(PlayerStatsPage playerStats)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/player.stats"; //"C:/System/";
        FileStream fs = new FileStream(path, FileMode.Create);

        DataToStore data = new DataToStore(playerStats);

        formatter.Serialize(fs, data);
        fs.Close();
    }

    public static DataToStore LoadData()
    {
        string path = Application.persistentDataPath + "/player.stats";
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);

            formatter.Deserialize(fs);
            DataToStore data = formatter.Deserialize(fs) as DataToStore;
            fs.Close();
            return data;
        } catch (Exception e)
        {
            Debug.Log("file " + path + " not found - Error : " + e);
            
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
