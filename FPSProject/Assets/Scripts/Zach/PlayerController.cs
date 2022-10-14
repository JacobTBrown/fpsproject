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
   
    }

    void Start()
    {
        	pManager = FindObjectOfType<PlayerManager>();
	        pManager.RegisterPlayer(gameObject);

    }

}
