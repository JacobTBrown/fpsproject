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
        Debug.Log("pmanager awake " + this);
        myPV = gameObject.GetComponent<PhotonView>();
        //Invoke("SetTeams", 0.5f);
   
       // team = (int)PhotonNetwork.LocalPlayer.CustomProperties["team"];
    }
    

    void Start()
    {
        
            Debug.Log(FindObjectOfType<PlayerManager>().name + " PlayerController.cs start");
            pManager = FindObjectOfType<PlayerManager>();
            pManager.RegisterPlayer(gameObject);
            Debug.Log("pmanager : " + pManager.name);
        }
    
    public void SetTeams()
    {
        if (!myPV.IsMine)
        {
            Debug.Log("return early from setTeams");
            //return;
        }
        //var player1 = PhotonNetwork.CurrentRoom.Players.ElementAt(0);
        GameObject[] ga = (GameObject.FindGameObjectsWithTag("Player"));
        List<PlayerDamageable> pd = new List<PlayerDamageable>();
        foreach (GameObject g in ga)
            if (g.GetComponent<PlayerDamageable>() != null)
            {
                Debug.Log("adding" + g.GetComponent<PlayerDamageable>().gameObject.transform.parent.name);
                pd.Add(g.GetComponent<PlayerDamageable>());
            }
        
        Debug.Log("list :" );
        foreach (Player pp in PhotonNetwork.PlayerList)
        {
            Debug.Log(pp.NickName + " is in playerList");
            if (pp == null)
            {
                Debug.Log("null player?");
            }
            if ((int)pp.CustomProperties["team"] == 2)
            {
                Debug.Log(pp.ActorNumber + " was on team 2");
                //setMaterialTeam2(pp);
                setMaterial();
            }
        }
    }
    public void setMaterial()
    {
        Debug.Log("matching player found");
        MeshRenderer[] m = gameObject.GetComponentsInChildren<MeshRenderer>();

        Debug.Log("got mesh = " + m[1].gameObject.name);
        Material[] materials = new Material[1];
        materials = m[1].materials;
        materials[0] = materials[0] = (Material)Resources.Load("materials/Player_Mat1");
        Debug.Log("size of materials arr = " + materials.Length);
        Debug.Log(materials[0]);

        Material[] playerMaterials = materials;
        Debug.Log("new player materials being set on mesh render = " + playerMaterials[0]);
        m[1].gameObject.GetComponent<MeshRenderer>().materials = playerMaterials;
        Debug.Log("meshRenderer was " + m[1].gameObject.GetComponent<MeshRenderer>().name);
    }
    private void setMaterialTeam2(Player p)
    {
       // foreach (Player x in PhotonNetwork.PlayerList) {
            
            Debug.Log("player nickname: " + p.NickName);
            GameObject[] g = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] playerBodies = new GameObject[g.Length];
        for (int i = 0; i < g.Length; i++)
            {
                if (g[i].GetComponent<PlayerDamageable>() != null)
                {
                    Debug.Log("got a playerdamage component");
                    playerBodies[i] = g[i].GetComponent<PlayerDamageable>().gameObject;
                Debug.Log(p.ActorNumber + " player's actor number");
                //  Debug.Log(playerBodies[i].gameObject.transform.parent.gameObject.name + " player body's parent's object name");
                Debug.Log(playerBodies[i].gameObject.name + " player body's  object name");
                Debug.Log(playerBodies[i].gameObject.transform.parent + " player body's parent name");
                if (p.ActorNumber == playerBodies[i].GetComponentInParent<PhotonView>().ControllerActorNr)
                {
                    Debug.Log("matching player found");
                    MeshRenderer[] m = gameObject.GetComponentsInChildren<MeshRenderer>();

                    Debug.Log("got mesh = " + m[1].gameObject.name);
                    Material[] materials = new Material[1];
                    materials = m[1].materials;
                    materials[0] = materials[0] = (Material)Resources.Load("materials/Player_Mat1");
                    Debug.Log("size of materials arr = " + materials.Length);
                    Debug.Log(materials[0]);

                    Material[] playerMaterials = materials;
                    Debug.Log("new player materials being set on mesh render = " + playerMaterials[0]);
                    m[1].gameObject.GetComponent<MeshRenderer>().materials = playerMaterials;
                    Debug.Log("meshRenderer was " + m[1].gameObject.GetComponent<MeshRenderer>().name);
                }
            }
           
            }



/*
       PlayerDamageable playerBodyScript = GetComponentInChildren<PlayerDamageable>();
            GameObject playerBodies = GameObject.Find("Player Body");
            Debug.Log(p.ActorNumber + " vs " + g[j]);
            if (p.ActorNumber == g[i].GetComponent<PhotonView>().ControllerActorNr && g[i].name == "Player Body")
            {
                Debug.Log(p.ActorNumber + " vs " + g[i].GetComponent<PhotonView>().ControllerActorNr + " body : " + g[i].name);
            }
 */
               
           
      //  }
    }
}
