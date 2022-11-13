using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script is used on the Change Team button in the room menu for TDM games
public class ChangeTeam : MonoBehaviour
{
    private Player _player;
    public int team;
   
    void Start()
    {
        Player _player = PhotonNetwork.LocalPlayer;
    }
    public void onclickChangeTeam()
    {
        Player p = PhotonNetwork.LocalPlayer;
        team = (int)PhotonNetwork.LocalPlayer.CustomProperties["team"];
        Launcher.Instance.photonView.RPC("changeTeamsRPC", RpcTarget.All, p, team);
    }
}
