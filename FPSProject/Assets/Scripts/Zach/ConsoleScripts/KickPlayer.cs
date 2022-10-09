using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickPlayer : MonoBehaviourPunCallbacks
{
    List<PhotonView> pvList = new List<PhotonView>();
    List<Player> playerInfoList = new List<Player>();
    List<string> playerIDList = new List<string>();
    Player[] plist;
    Player player;
    // Start is called before the first frame update
    void Start()
    {

        plist = PhotonNetwork.PlayerList;   
        for (int i = 0; i < plist.Length; i++)
        {
            if (plist[i].UserId.Length > 0)
            {
                playerInfoList.Add(plist[i]);
                //plist[i].UserId
            }
            Debug.Log(plist[i]);
        }
       // pvList =    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [PunRPC]
    private void KickPlayerCMD(string playerID)
    {
        if (PhotonNetwork.LocalPlayer.UserId == playerID)
            PhotonNetwork.LeaveRoom();
    }
    public void SendKickPlayer(int playerID)
    {
        //foreach (PhotonView id in )
        // Player kickPlayer = playerInfoList.Find(playerID);

        photonView.RPC("KickPlayerCMD", RpcTarget.All, playerID);
    }
}
