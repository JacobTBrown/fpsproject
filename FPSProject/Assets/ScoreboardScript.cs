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

    public void Initialize(Player player)
    {
        Debug.Log("Player name is: " + player.NickName);
        usernameText.text = player.NickName;
    }
}