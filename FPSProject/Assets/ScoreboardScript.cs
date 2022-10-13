using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class ScoreboardScript : MonoBehaviour
{
    public TMP_Text usernameText;
    public TMP_Text killsText;
    public TMP_Text deathsText;
    private Player player;

    public void Initialize(Player _player)
    {
        Debug.Log("Player name is: " + _player.NickName);
        player = _player;
        usernameText.text = _player.NickName;
    }

    private void Update() {
        usernameText.text = player.NickName;
    }
}