using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Unity.Scripts.Jonathan;
using Photon.Pun;
using ExitGames.Client.Photon;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class ScoreboardScript : MonoBehaviour
{
    PlayerStatsPage statsPage;
    public TMP_Text usernameText;
    public TMP_Text killsText;
    public TMP_Text deathsText;
    public int deaths;
    public int kills;
    private Player player;
    PhotonView PPV;
    PhotonView EPV;

    public void Initialize(Player _player)
    {
        //Debug.Log("Player name is: " + _player.NickName);
        player = _player;
        usernameText.text = _player.NickName;
    }

    private void Update()
    {
        usernameText.text = player.NickName;
    }

    private void Awake()
    {
        EventManager.AddListener<updateScore>(onUpdate);
    }

    public void onUpdate(updateScore obj)
    {
        kills = (int)player.CustomProperties["Kills"];
        deaths = (int)player.CustomProperties["Deaths"];
        deathsText.text = deaths.ToString();
        killsText.text = kills.ToString();

        Debug.Log("Score: "+(int)player.CustomProperties["Kills"]);
    }


    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        Debug.Log("In the OnEvent function in Scoreboard Script.");
        byte eventCode = photonEvent.Code;
        if (eventCode == PhotonEvents.PLAYERDEATH)
        {
            //Debug.Log("Sender: " + photonEvent.Sender.ToString());
            object[] data = (object[])photonEvent.CustomData;
            int EnemyPlayer = (int)data[1]; //the photon view of the person who dealt damage
            //Debug.Log(data[1].ToString() + " was enemy player data");
            EPV = PhotonNetwork.GetPhotonView(EnemyPlayer);
            //Debug.Log("Enemy player was: " + EnemyPlayer.ToString() + " vs my actor #: " + PhotonNetwork.LocalPlayer.ActorNumber);
            PPV = PhotonNetwork.GetPhotonView((int)data[0]);
            if (PPV.IsMine)
            {
                Debug.Log("Set deaths manually for: " + PhotonNetwork.LocalPlayer.ActorNumber);
                statsPage.setDeaths();
            }
            else if (EPV.IsMine)
            {
                Debug.Log("Set kills manually for: " + PhotonNetwork.LocalPlayer.ActorNumber);
                statsPage.SetKills();
            }
        }
    }


}