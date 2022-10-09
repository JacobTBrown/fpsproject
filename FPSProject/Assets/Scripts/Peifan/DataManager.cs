using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public string UserId = "";
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

    public string GetUserID()
    {
        return UserId;
    }
}
