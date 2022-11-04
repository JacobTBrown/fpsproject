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
    int stop;
   [SerializeField] double timerIncrementValue;
    double startTime;
    [SerializeField] double timer;
    [SerializeField] public TextMeshProUGUI TimerText;
    [SerializeField] public TMP_Text GameOverText;
    ExitGames.Client.Photon.Hashtable CustomeValue;
 
 void Start()
     {
        stop = 0;
         timer = 666;
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
            Invoke("StartTimeForOthers", 1);   
         }
     }
    public void StartTimeForOthers()
    {
        Debug.Log("room props: " + PhotonNetwork.CurrentRoom.CustomProperties["StartTime"]);
        startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
        startTimer = true;
    }
 
     void Update()
     {
 
         if (!startTimer) return;

         timerIncrementValue = (int)((PhotonNetwork.ServerTimestamp - startTime)/1000);
         int mins = (int)timerIncrementValue/60;
         int seconds = (int)timerIncrementValue - (mins*60);
         TimerText.text = mins.ToString("D1") + ":" + seconds.ToString("D2");

        if (timerIncrementValue >= timer && stop == 0)
        {
            TimesUP(); // is running on update??
            stop = 1;
            startTimer = false;
        }
        
    }

     void TimesUP()
     {
          EndGameEvent ev = Events.EndGameEvent;
          EventManager.Broadcast(ev);
          gameObject.SetActive(false);
        GameOverText.text += "\n Time expired! No winner!";
        //start an exit game routine here if we have one: (exit game after a short delay)
    }

}