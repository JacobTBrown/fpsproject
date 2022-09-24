using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    //on player join, add player to list.
    //on plyaer leave, remove from list
    [SerializeField] TMP_Text text;
    Player player;
    // Start is called before the first frame update
    public void SetUp(Player _player)
    {
        player = _player;
        text.text = _player.NickName;
    }

    // Update is called once per frame
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
