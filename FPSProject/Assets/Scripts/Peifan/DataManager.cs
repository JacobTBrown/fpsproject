using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public string UserId = "";
    public string RoomName = "";
    public bool IsCanShoot = true;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetUserID(string name)
    {
        UserId = name;
    }

    public void SetRoomName(string roomName) 
    {
        RoomName = roomName;
    }

    public string GetUserID()
    {
        return UserId;
    }

    public string GetRoomName() 
    {
        return RoomName;
    }
}
