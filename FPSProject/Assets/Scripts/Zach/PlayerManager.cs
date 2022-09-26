using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    [SerializeField] GameObject SettingsPanel;
    PlayerSettings playerSettings;
    GameObject canvas;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        //SettingsPanel = 

    }

    private void Start()
    {
        if (PV.IsMine)
        {
            Debug.Log("Creating controller for player:" + PV.ViewID);
            
            createController();
        }
    }
    void createController()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Vector3.zero, Quaternion.identity);
        //this should create a settings panel for each prefab
        //zach/jacob
        playerSettings = GameObject.Find("Player(Clone)").GetComponent<PlayerSettings>();
        //Instantiate(playerSettings.settingPanel, GameObject.Find("Canvas").GetComponent<Transform>());
        //GameObject.Find("SettingPanel(Clone)").SetActive(false);
    }//get the playersettings script form the object
    //retrieve the settings panel, save that object,
    //instantiate that object
}
