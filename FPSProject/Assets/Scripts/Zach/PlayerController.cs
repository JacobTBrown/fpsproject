using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerManager pManager;
    public PhotonView PV;

    void Awake()
    {
   
    }

    void Start()
    {
        	pManager = FindObjectOfType<PlayerManager>();
	        pManager.RegisterPlayer(gameObject);

    }

}
