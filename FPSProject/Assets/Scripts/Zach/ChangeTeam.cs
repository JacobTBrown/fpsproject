using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTeam : MonoBehaviour
{
    private Player _player;
    public int team;
    // Start is called before the first frame update
    void Start()
    {
        Player _player = PhotonNetwork.LocalPlayer;
    }
    public void onclickChangeTeam()
    {
        Player p = PhotonNetwork.LocalPlayer;
        team = (int)PhotonNetwork.LocalPlayer.CustomProperties["team"];
        Launcher.Instance.photonView.RPC("changeTeamsRPC", RpcTarget.All, p, team);
/*        team = (int)_player.CustomProperties["team"];
        if (team == 0)
        {
            team = 1;
        } else if (team == 1)
        {
            team = 2;
        }
        else if (team == 2)
        {
            team = 1;
        }
        _player.CustomProperties["team"] = team;*/
    }
}
