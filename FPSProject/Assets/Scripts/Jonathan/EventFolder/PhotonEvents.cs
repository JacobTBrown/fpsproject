using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
namespace Unity.Scripts.Jonathan
{
    public class PhotonEvents : MonoBehaviour 
    {
        /*
            Creator: Jonathan

            PhotonNetWork Events only allow the customData type to be
            object[] annoying but workable.

        */       
        public const byte PLAYERDEATH = 0; 
        private void OnEnable()
        {
            PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        }

        private void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
        }

        private void OnEvent(EventData photonEvent){
            /*
                This assumes that Obj[0] is the players veiwID
            */
            Debug.Log("OnEvent called for " + photonEvent.CustomData);
            if (photonEvent.Code.ToString().Length == 0)
            {
                Debug.Log("empty event " + photonEvent.CustomData);
            }
            byte eventCode = photonEvent.Code;
            object[] player = (object[])photonEvent.CustomData;

            if(eventCode == PLAYERDEATH){OnPlayerDeath(PhotonView.Find((int)player[0]).gameObject);}
        }

        private void OnPlayerDeath(GameObject player){

            Debug.Log("OnPlayerDeath was called");
            Debug.Log("TEST: NEW PLAYER KILL EVENT FOR " + player);
            /*
                I have player kill and death be default event. IT's unkown what other program needs 
                so beware of some programms requiring PlayerDeathEvents and 
                others needing PlayerKillEvents
            */
            PlayerDeathEvent playerDeathEvent = Events.PlayerDeathEvent;
            playerDeathEvent.player = player;
            EventManager.Broadcast(playerDeathEvent);

            PlayerKillEvent playerKillEvent = Events.PlayerKillEvent;
            playerKillEvent.player = player;
            EventManager.Broadcast(playerKillEvent);
        }
    }
}
