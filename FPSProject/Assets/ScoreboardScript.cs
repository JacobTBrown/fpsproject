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
    }

    public void onUpdate(updateScore obj)
    {
        kills = (int)player.CustomProperties["Kills"];
        deaths = (int)player.CustomProperties["Deaths"];
        deathsText.text = deaths.ToString();
        killsText.text = kills.ToString();
        if (player.IsLocal)
        {
            FindObjectOfType<KillHUD>().updateText("kills: " + kills.ToString());
        }

        
    }

    public void OnPlayerKill(PlayerKillEvent evt)
    {
        // Debug.Log("Entered ScoreboardScript.cs OnPlayerKill");
        // Debug.Log("This player is: " + player.NickName + "Killed player is: " + evt.player.GetComponent<PlayerSettings>().nickname);
        if (evt.player.GetComponent<PlayerSettings>().nickname.Equals(player.NickName))
        {
            //kills += 1;
            //killsText.text = kills.ToString();
            Debug.Log("Number of kills are: " + kills);
        }
    }

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == PhotonEvents.PLAYERDEATH)
        {
            //Debug.Log("Sender: " + photonEvent.Sender.ToString());
            object[] data = (object[])photonEvent.CustomData;
            int EnemyPlayer = (int)data[1]; //the photon view of the person who dealt damage
            //Debug.Log(data[1].ToString() + " was enemy player data");
            EPV = PhotonNetwork.GetPhotonView(EnemyPlayer);
            PPV = PhotonNetwork.GetPhotonView((int)data[0]);
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        //Debug.Log("In here for changing properties.");
        kills = (int)player.CustomProperties["Kills"];
        deaths = (int)player.CustomProperties["Deaths"];
        deathsText.text = deaths.ToString();
        killsText.text = kills.ToString();
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
    }

}