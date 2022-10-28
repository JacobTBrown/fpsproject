using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerManager pManager;

    void Awake()
    {
        Debug.Log("pmanager awake " + this);

    }


    void Start()
    {
        Debug.Log(FindObjectOfType<PlayerManager>().name + " PlayerController.cs start");
        	pManager = FindObjectOfType<PlayerManager>();
	        pManager.RegisterPlayer(gameObject);
        Debug.Log("pmanager : " + pManager.name);

    }

}
