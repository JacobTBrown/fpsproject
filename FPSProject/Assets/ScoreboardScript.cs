﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Unity.Scripts.Jonathan;
using Photon.Pun;
using ExitGames.Client.Photon;
using System;

public class ScoreboardScript : MonoBehaviourPunCallbacks, IOnEventCallback
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
        EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);
        EventManager.AddListener<PlayerKillEvent>(OnPlayerKill);
    }

    private void onUpdate(updateScore obj)
    {
        kills = (int)player.CustomProperties["Kills"];
        deaths = (int)player.CustomProperties["Deaths"];
        deathsText.text = deaths.ToString();
        killsText.text = kills.ToString();
    }

    public void OnPlayerDeath(PlayerDeathEvent evt)
    {
        //HandlePlayerSpawn(evt.player);
        Debug.Log("This player is: " + player.NickName + " Dead player is: " + evt.player.GetComponent<PlayerSettings>().nickname);
        if (evt.player.GetComponent<PlayerSettings>().nickname.Equals(player.NickName))
        {
            //deaths += 1;
            //deathsText.text = deaths.ToString();
            //Debug.Log("Number of deaths are: " + deaths);
        }

        //kills = (int)player.CustomProperties["Kills"];
        //deaths = (int)player.CustomProperties["Deaths"];
        //deathsText.text = deaths.ToString();
        //killsText.text = kills.ToString();
    }

    public void OnPlayerKill(PlayerKillEvent evt)
    {
        Debug.Log("Entered ScoreboardScript.cs OnPlayerKill");
        Debug.Log("This player is: " + player.NickName + "Killed player is: " + evt.player.GetComponent<PlayerSettings>().nickname);
        if (evt.player.GetComponent<PlayerSettings>().nickname.Equals(player.NickName))
        {
            //kills += 1;
            //killsText.text = kills.ToString();
            Debug.Log("Number of kills are: " + kills);
        }

        //kills = (int)player.CustomProperties["Kills"];
        //deaths = (int)player.CustomProperties["Deaths"];
        //deathsText.text = deaths.ToString();
        //killsText.text = kills.ToString();
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

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        Debug.Log("In here for changing properties.");
        kills = (int)player.CustomProperties["Kills"];
        deaths = (int)player.CustomProperties["Deaths"];
        deathsText.text = deaths.ToString();
        killsText.text = kills.ToString();
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
    }
}