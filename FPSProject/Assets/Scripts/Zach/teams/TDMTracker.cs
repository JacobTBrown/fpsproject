using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TDMTracker : MonoBehaviourPunCallbacks
{
    public Player[] players;
    public int team1Kills;
    public int team2Kills;
    public Text EnemyText;
    // Start is called before the first frame update
    void Awake()
    {

        // int team1Kills = GameObject.Find("GameManager").GetComponent<TDMKillObjective>().team1Kills + GameObject.Find("ScoreboardTeams").GetComponent<ScoreboardTeams>().team1Size;
        //  int team2Kills = GameObject.Find("GameManager").GetComponent<TDMKillObjective>().team2Kills + GameObject.Find("ScoreboardTeams").GetComponent<ScoreboardTeams>().team2Size;

    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {

        if (!changedProps.ContainsKey("Kills"))
        {
            return;
        }
        int team1Kills = 0;
        int team2Kills = 0;
        Text EnemyText = GameObject.Find("Enemy").GetComponent<Text>();
        players = PhotonNetwork.PlayerList;
        foreach (Player player in players)
        {
            if ((int)player.CustomProperties["team"] == 1)
            {
                team1Kills += (int)player.CustomProperties["Kills"];
            }
            else if ((int)player.CustomProperties["team"] == 2)
            {
                team2Kills += (int)player.CustomProperties["Kills"];
            }
            if (team1Kills > team2Kills)
            {
                EnemyText.text = "Team1 with:  " + team1Kills;
            }
            else if (team1Kills < team2Kills)
            {
                EnemyText.text = "team2 with:  " + team2Kills;
            }
            else
            {
                EnemyText.text = "Tie Game!";
            }
        }
    }
}
