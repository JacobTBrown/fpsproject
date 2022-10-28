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
        public const byte JOINEDTEAM1 = 70;
        public const byte JOINEDTEAM2 = 71;

        private void OnEnable()
        {
            PhotonNetwork.NetworkingClient.EventReceived += OnEvent1;
        }
    
    

        private void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= OnEvent1;
        }

        private void OnEvent1(EventData photonEvent){
            /*
                This assumes that Obj[0] is the players veiwID
            */
            //Debug.Log("OnEvent called for " + photonEvent.CustomData);
              //Debug.Log("event code : " + photonEvent.Code.ToString());
            
            byte eventCode = photonEvent.Code;
            //if ((bool)photonEvent.CustomData);
            
 
            if (eventCode == PLAYERDEATH){
                object[] player = (object[])photonEvent.CustomData;
                Debug.Log(PhotonView.Find((int)player[0]).gameObject + " was photonEvent.cs's player game obj");
                PhotonView enemyPV = PhotonView.Find((int)player[1]);
                Debug.Log("Custom event data " + photonEvent.CustomData.ToString()); 
                OnPlayerDeath(PhotonView.Find((int)player[0]).gameObject);
            }
        }

        private void OnPlayerDeath(GameObject player){

            Debug.Log("OnPlayerDeath was called");
            Debug.Log("TEST: NEW PLAYER KILL EVENT FOR " + player.GetPhotonView().ViewID);
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
