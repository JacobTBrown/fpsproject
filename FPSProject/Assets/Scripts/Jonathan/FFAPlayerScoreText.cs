using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Scripts.Jonathan;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class FFAPlayerScoreText : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]Text PlayerText;
    [SerializeField]Text EnemyText;
    void Start()
    {
    //    Text[] txt= GetComponentsInChildren<Text>();
    //     EnemyText = txt[0];
    //     PlayerText = txt[1];
        EventManager.AddListener<updateScore>(UpdateScore);
        UpdateKillScore();
        if ((int)PhotonNetwork.LocalPlayer.CustomProperties["team"] > 0)
        {
            gameObject.AddComponent<TDMTracker>();
            EventManager.RemoveListener<updateScore>(UpdateScore);
        }
    }
    public void UpdateScore(updateScore evt)
    {
        UpdateKillScore();
    }

    public void UpdateKillScore()
    {
        PlayerText.text = " Player Score: " + (int)PhotonNetwork.LocalPlayer.CustomProperties["Kills"];  
        Player[] playerList = PhotonNetwork.PlayerList;
        Player Enemy = playerList[0];
        foreach (Player player in playerList)
        {
            if((int)Enemy.CustomProperties["Kills"] < (int)player.CustomProperties["Kills"])
            {
                Enemy = player;
            }
        }
        int PlayerScore = (int)PhotonNetwork.LocalPlayer.CustomProperties["Kills"];
        int EnemyScore = (int)Enemy.CustomProperties["Kills"];
        PlayerText.text = " Player Score: " + PlayerScore; 
        EnemyText.text = "Highest Score: " + EnemyScore;
        //print(EnemyScore);
    }
}
