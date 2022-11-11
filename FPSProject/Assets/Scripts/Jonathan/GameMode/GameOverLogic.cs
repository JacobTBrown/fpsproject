using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Scripts.Jonathan;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class GameOverLogic : MonoBehaviour
{
    public Image endScreen;
    private Tween fadeTween;
    [SerializeField] public TextMeshProUGUI GameOverText;
    // Start is called before the first frame update
    void Start()
    {
        GameOverText = GetComponent<TextMeshProUGUI>();
        EventManager.AddListener<EndGameEvent>(onGameOver);
        EventManager.AddListener<ObjectiveCompletedEvent>(onOjectiveComplete);
        gameObject.SetActive(false);
        endScreen = gameObject.GetComponentInParent<Image>();    
        endScreen.gameObject.SetActive(false);    
    }

    public void onGameOver(EndGameEvent evt){
        gameObject.SetActive(true);
        gameObject.transform.parent.gameObject.SetActive(true);
        FadeInFadeOut(10.0f, 10.0f);
        Invoke("ClearEndOfGame", 10f);
        EventManager.RemoveListener<EndGameEvent>(onGameOver);
    }
    
    private void FadeInFadeOut(float endValue, float duration)
    {
        endScreen.DOFade(255, duration).OnComplete(() => { 
            endScreen.DOFade(0, endValue);
        });
    }

    public void ClearEndOfGame()
    {
        gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);
        //exit button 
    }

    public void onOjectiveComplete(ObjectiveCompletedEvent evt)
    {
        if(evt.objective.GetType() == typeof(FreeForAllKillObjective)) SetFFAGameOverText(GameOverText, evt);
        if(evt.objective.GetType() == typeof(TDMKillObjective)) SetTDMGameOverText(GameOverText, evt);
    }
    public void SetFFAGameOverText(TextMeshProUGUI GameOverText,ObjectiveCompletedEvent evt)
    {
        Player[] playerList = PhotonNetwork.PlayerList;
        
        for(int i = 0; i < playerList.Length; i++)
        {
            for(int j = 0; j < playerList.Length; j++)
            {
                if((int)playerList[i].CustomProperties["Kills"] < (int)playerList[j].CustomProperties["Kills"])
                {
                    Player temp = playerList[i];
                    playerList[i]=playerList[j];
                    playerList[j]=playerList[i];
                }
            }
        }

        string str = "Game Over! \n";
        int Count = 1;
        foreach(Player player in playerList)
        {
            str += Count + ": " + player.NickName;
            if(Count == 1) str += " Wins!";
            str += "\n";
            Count++;
        }
        GameOverText.text = str;
    }

    public void SetTDMGameOverText(TextMeshProUGUI GameOverText,ObjectiveCompletedEvent evt)
    {
        string str = "Game Over! \n";
        TDMKillObjective obj = (TDMKillObjective)evt.objective;
        if(obj.team1Kills > obj.team2Kills)
            str += "Team 1 Wins!" ;
        else
            str += "Team 1 Wins!";
        GameOverText.text = str;
    }
}