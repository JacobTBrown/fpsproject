using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform container;
    [SerializeField] GameObject scoreboardItemPrefab;

    Dictionary<Player, ScoreboardScript> scoreboardItems = new Dictionary<Player, ScoreboardScript>();

    private void Start()
    {
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            AddScoreboardItem(player);
        }
    }

    void AddScoreboardItem(Player player)
    {
        ScoreboardScript item = Instantiate(scoreboardItemPrefab, container).GetComponent<ScoreboardScript>();
        item.Initialize(player);
        scoreboardItems[player] = item;
        if (player.IsLocal)
        {
            item.gameObject.GetComponentInChildren<Image>().color = new Color(0, 0, 0, .90f);
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