using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Realtime;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerManager pManager;
    int team;
    PhotonView myPV;
    //Player player;
    void Awake()
    {
        myPV = gameObject.GetComponent<PhotonView>();
        //Invoke("SetTeams", 0.5f);
   
       // team = (int)PhotonNetwork.LocalPlayer.CustomProperties["team"];
    }
    

    void Start()
    {
            pManager = FindObjectOfType<PlayerManager>();
            pManager.RegisterPlayer(gameObject);      
        }
    
    public void SetTeams()
    { //settings team materials in here because it has easy access to the player body -zach
        if (!myPV.IsMine)
        {
            //return;
        }
        //var player1 = PhotonNetwork.CurrentRoom.Players.ElementAt(0);
        GameObject[] ga = (GameObject.FindGameObjectsWithTag("Player"));
        List<PlayerDamageable> pd = new List<PlayerDamageable>();
        foreach (GameObject g in ga)
            if (g.GetComponent<PlayerDamageable>() != null)
            {
                pd.Add(g.GetComponent<PlayerDamageable>());
            } 
        foreach (Player p in PhotonNetwork.PlayerList)
        {      
            if ((int)p.CustomProperties["team"] == 2)
            {
                //Debug.Log(p.ActorNumber + " was on team 2");
                setMaterial();
            }
        }
    }
    public void setMaterial()
    {
        MeshRenderer[] m = gameObject.GetComponentsInChildren<MeshRenderer>();
        Material[] materials = new Material[1];
        materials = m[1].materials;
        materials[0] = materials[0] = (Material)Resources.Load("materials/Player_Mat1");
        //Debug.Log(materials[0]);

        Material[] playerMaterials = materials;
        m[1].gameObject.GetComponent<MeshRenderer>().materials = playerMaterials;
    }
}
