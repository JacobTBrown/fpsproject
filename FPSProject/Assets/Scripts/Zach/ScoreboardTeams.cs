using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class ScoreboardTeams : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform team1Container;
    [SerializeField] Transform team2Container;
    [SerializeField] GameObject scoreboardItemPrefab;

    Dictionary<Player, ScoreboardScript> scoreboardItems = new Dictionary<Player, ScoreboardScript>();

    private void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddScoreboardItem(player);
        }
    }

    void AddScoreboardItem(Player player)
    {
        if ((int)player.CustomProperties["team"] == 1) {
        ScoreboardScript item = Instantiate(scoreboardItemPrefab, team1Container).GetComponent<ScoreboardScript>();
            item.Initialize(player);
            scoreboardItems[player] = item;
        } else if ((int)player.CustomProperties["team"] == 2)
        {
            ScoreboardScript item = Instantiate(scoreboardItemPrefab, team2Container).GetComponent<ScoreboardScript>();
            item.Initialize(player);
            scoreboardItems[player] = item;
        }
       
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddScoreboardItem(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemoveScoreboardItem(otherPlayer);
    }

    public void RemoveScoreboardItem(Player player)
    {
        Destroy(scoreboardItems[player].gameObject);
        scoreboardItems.Remove(player);
    }
}