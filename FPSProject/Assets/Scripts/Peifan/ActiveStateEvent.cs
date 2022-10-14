using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveStateEvent : MonoBehaviour
{

    public GameObject SettingPanel;
    public GameObject ChatRoom;

    // Update is called once per frame
    void Update()
    {
        if (SettingPanel.gameObject.activeInHierarchy) DataManager.Instance.IsCanShoot = false;
        else DataManager.Instance.IsCanShoot = true;
        if (ChatRoom.gameObject.activeInHierarchy) DataManager.Instance.IsCanShoot = false;
        else DataManager.Instance.IsCanShoot = true;
    }
}

