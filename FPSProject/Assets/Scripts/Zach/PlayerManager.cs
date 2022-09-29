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
    Vector3 initSpawn = new Vector3(0, 5, 0);
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
       
        //SettingsPanel = 

    }
    CharacterController cc;
    private void Start()
    {
        if (PV.IsMine)
        {
            createController();
        }
        if (!PV.IsMine)
        {
           //GetComponentInChildren<Camera>().enabled = false;
          // GetComponentInChildren<AudioListener>().enabled = false;
        }//  Calling Destroy in PlayerMovement instead - Zach 9-28
    }
    void createController()
    {
        
        //I moved this part to the Game Manager
        Debug.Log("PlayerManager.cs called createController()!");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), initSpawn, Quaternion.identity);
        //this should create a settings panel for each prefab
        //zach/jacob
        //playerSettings = GameObject.Find("Player(Clone)").GetComponent<PlayerSettings>();
        //Instantiate(playerSettings.settingPanel, GameObject.Find("Canvas").GetComponent<Transform>());
        //GameObject.Find("SettingPanel(Clone)").SetActive(false);
    }
}
