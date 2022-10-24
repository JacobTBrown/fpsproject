using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Unity.Scripts.Jonathan;
using Photon.Pun;

public class ScoreboardScript : MonoBehaviour
{
    public TMP_Text usernameText;
    public TMP_Text killsText;
    public TMP_Text deathsText;
    public int deaths;
    public int kills;
    private Player player;

    public void Initialize(Player _player)
    {
        //Debug.Log("Player name is: " + _player.NickName);
        player = _player;
        usernameText.text = _player.NickName;
    }

    private void Update() {
        usernameText.text = player.NickName;
    }

    private void Awake()
    {
        EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);
        EventManager.AddListener<PlayerKillEvent>(OnPlayerKill);
    }

    public void OnPlayerDeath(PlayerDeathEvent evt)
    {
        //HandlePlayerSpawn(evt.player);
        //Debug.Log("This player is: " + player.NickName + " Dead player is: " + evt.player.GetComponent<PlayerSettings>().nickname);
        if (evt.player.GetComponent<PlayerSettings>().nickname.Equals(player.NickName))
        {
            deaths += 1;
            deathsText.text = deaths.ToString();
            Debug.Log("Number of deaths are: " + deaths);
        }
    }

    public void OnPlayerKill(PlayerKillEvent evt)
    {
        Debug.Log("Entered ScoreboardScript.cs OnPlayerKill");
        Debug.Log("This player is: " + player.NickName + "Killed player is: " + evt.playerWhoKilled.GetComponent<PlayerSettings>().nickname);
        if (evt.playerWhoKilled.GetComponent<PlayerSettings>().nickname.Equals(player.NickName))
        {
            kills += 1;
            killsText.text = kills.ToString();
            Debug.Log("Number of kills are: " + kills);
        }
    }
}