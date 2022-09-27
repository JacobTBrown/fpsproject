using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



//[RequireComponent(typeof(CharacterController))] refactor this

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
    CharacterController cc;
    private void Start()
    {
        if (!PV.IsMine)
        {
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
        }

/*        if (PV.IsMine)
        {
            Debug.Log("Creating controller for player:" + PV.ViewID);  
            createController(); //not currently being used
        }*/
    }
    void createController()
    {
        //I moved this part to the Game Manager

        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Vector3.zero, Quaternion.identity);
        //this should create a settings panel for each prefab
        //zach/jacob
        //playerSettings = GameObject.Find("Player(Clone)").GetComponent<PlayerSettings>();
        //Instantiate(playerSettings.settingPanel, GameObject.Find("Canvas").GetComponent<Transform>());
        //GameObject.Find("SettingPanel(Clone)").SetActive(false);
    }
}
