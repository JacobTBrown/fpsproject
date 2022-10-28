using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerListItemTeam : MonoBehaviourPunCallbacks
{
    //on player join, add player to list.
    //on plyaer leave, remove from list
    [SerializeField] Text numberText;
    [SerializeField] Text text;
    [SerializeField] Text teamText;
    Player player;
    // Start is called before the first frame update
    public void SetUp(Player _player)
    {
        Player[] players = PhotonNetwork.PlayerList;
        for (int i =0; i< players.Length; i++)
        {
            if (_player == players[i])
            {
                numberText.text = i.ToString();
            }
        }
        player = _player;
        text.text = _player.NickName;
        teamText.text = _player.CustomProperties["team"].ToString();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
