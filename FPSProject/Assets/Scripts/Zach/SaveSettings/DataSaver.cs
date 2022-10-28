using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using Photon.Pun;

public static class DataSaver
{
    //Zach 10-10
    // PlayerStatsPage tells this file when to save data.
    // Data from PlayerStatsPage is serialized by DataToStore,
    // then the serialized data is saved to C:\Users\"UserName"\AppData\LocalLow\DefaultCompany\FPSProject\player.stats

    private static bool debug = false;
    public static void SaveStats(DataToStore data)
    {
        
        //Debug.Log("saving from DataSaver.cs");
        
        //string path = Path.Combine(Application.persistentDataPath, "player.stats2");
        string path = Application.persistentDataPath + "/player.stats2";
        //Debug.Log("path was " + path);
        string json = JsonUtility.ToJson(data);
        if (debug) Debug.Log("You sent json: "+json);
        if (File.Exists(path))
        {
            // Take the exisintg Json from file -> convert to data -> data = oldData + data
            //FileStream fs = File.OpenRead(path);
            //Debug.Log("File Found");
            string oldJson = File.ReadAllText(path); // i dont actually need this part
            //if (debug) Debug.Log("Read previous data: " + oldJson);
            DataToStore oldData = JsonUtility.FromJson<DataToStore>(oldJson);
            DataToStore newData = new DataToStore(data, oldData); // newData = currentData + oldData
            json = JsonUtility.ToJson(newData);
            //json = JsonUtility.ToJson(File.ReadAllText(path));
            if (debug) Debug.Log(json);
            //JsonUtility.FromJsonOverwrite(json, data);
            //Debug.Log("Overwritten Json: " + json);
            //json utility adds the two data together
            //do the json overwriting
            //FileStream newFS = File.OpenWrite(path);
            File.WriteAllText(path, json);
            if (debug) Debug.Log("Finished saving new data");
            if (debug) Debug.Log("data was " + json);
            //fs.Dispose();
            //fs.Close();
            //Debug.Log("file closed");

        }
        else
        {
            //Debug.Log("file not found, saving json " + json);
            FileStream fs = File.Create(path);
            fs.Close();
            File.WriteAllText(path, json);
            fs.Dispose();
            //fs.Close();
            //Debug.Log("file closed");
            //create new file & save
        }
    }
    /// <summary>
    /// Returns data loaded from file. If no file is found, It creates a new file with your data and returns with the data you sent.
    /// </summary>
    public static DataToStore LoadData(DataToStore data)
    {
        //Debug.Log("entered DataSaver's loader");
        string path = Application.persistentDataPath + "/player.stats2";
        //string path = Path.Combine(Application.persistentDataPath, "player.stats2");
         //check existing -> read or create -> return data
            //Debug.Log("loading from dataSaver.cs");
            //Debug.Log("path was: " + path);
        if (File.Exists(path))
        { 
            try
            {
                
                FileStream fs = File.OpenRead(path);
                //Debug.Log("file was found and your json is -> ");
                fs.Dispose();
                //Debug.Log("that took " + Time.realtimeSinceStartup + " seconds");
                string json = File.ReadAllText(path);
                //Debug.Log("-> " + json);
                data = JsonUtility.FromJson<DataToStore>(json);
                if (fs.CanRead)
                {
                    //Debug.Log("fs still open");
                    fs.Close();
                }
                //Debug.Log("file closed");
            }
            catch (Exception e)
            {
                if (debug) Debug .Log("file was found, but Unity threw " + e);
            }
        }
        else
        {
            try
            {
                string json = JsonUtility.ToJson(data);
                //Debug.Log("making you a new file now with default values");
                FileStream fs = File.Create(path);
                fs.Close();
                File.WriteAllText(path, json);
                fs.Close();
                fs.Dispose();
                //Debug.Log("file closed");
            }

            catch (Exception e)
            {
                if (debug) Debug.Log("ERROR MESSAGE FROM UNITY: " + e);
                if (debug) Debug.Log("Path was: " + path);
                if (debug) Debug.Log("If this is your first time launching the game, you will have a new file on first auto-save");
                
            }
        }
        //Debug.Log("data was : " + data.timeInGame + " " + data.totalTime);//data.totalDeaths);
        return data; //! CHANGE TO NEW DATA PLS, ITS NO LONGER DEFAULT VALUES 

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
