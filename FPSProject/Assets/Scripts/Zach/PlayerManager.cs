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
    Vector3 initSpawn = new Vector3(0, 5, 0);
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    CharacterController cc;
    private void Start()
    {
        if (PV.IsMine)
        {
            createController();
        }

    }
    void createController()
    {

        Debug.Log("PlayerManager.cs called createController()!");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), initSpawn, Quaternion.identity);
    }   
}
