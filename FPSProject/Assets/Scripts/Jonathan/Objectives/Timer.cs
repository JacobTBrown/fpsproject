using System.Collections;
using System.Collections.Generic;
using Unity.Scripts.Jonathan;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Timer : MonoBehaviour
{
    bool startTimer = false;
   [SerializeField] double timerIncrementValue;
    double startTime;
    [SerializeField] double timer;
    [SerializeField] public TextMeshProUGUI TimerText;
    ExitGames.Client.Photon.Hashtable CustomeValue;
 
 void Start()
     {
         timer = 660;
         TimerText = GetComponent<TextMeshProUGUI>();
         if (PhotonNetwork.LocalPlayer.IsMasterClient)
         {
             CustomeValue = new ExitGames.Client.Photon.Hashtable();
             startTime = PhotonNetwork.ServerTimestamp;
             startTimer = true;
             CustomeValue.Add("StartTime", startTime);
             PhotonNetwork.CurrentRoom.SetCustomProperties(CustomeValue);
         }
         else
         {
             startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
             startTimer = true;
         }
     }
 
     void Update()
     {
 
         if (!startTimer) return;

         timerIncrementValue = (int)((PhotonNetwork.ServerTimestamp - startTime)/1000);
         int mins = (int)timerIncrementValue/60;
         int seconds = (int)timerIncrementValue - (mins*60);
         TimerText.text = mins.ToString("D1") + ":" + seconds.ToString("D2");
        int stop = 0;
         if (timerIncrementValue >= timer && stop == 0)
         {
            TimesUP();
            stop = 1;
         }
     }

     void TimesUP()
     {
                Debug.Log("Timer: End Game Event Called");
                EndGameEvent ev = Events.EndGameEvent;
                EventManager.Broadcast(ev);
     }
}