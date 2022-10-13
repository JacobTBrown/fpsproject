using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

//Zach - 10/10 -
// Updates the List of all rooms (FindRoomMenu)
// onRoomListUpdate is called every time someone leaves or joins.

public class RoomListingsMenuNEW : MonoBehaviourPunCallbacks
{

    [SerializeField] private Transform findRoomPanel;
    [SerializeField] private RoomListItem _roomListItemPrefab;

    List<RoomInfo> AllRoomsList = new List<RoomInfo>();
    private List<RoomListItem> _listings = new List<RoomListItem>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        
        //roomListItem.sizeText.text = roomInfo.PlayerCount.ToString();
        //_listings = roomList;
        //Debug.Log("new on room list update");
        foreach (RoomInfo updatedRoom in roomList)
        {
            Debug.Log("updating from roomListingMenu.cs");
            if (updatedRoom.PlayerCount == updatedRoom.MaxPlayers)
            {
                updatedRoom.RemovedFromList = true;
            }
            if (updatedRoom.RemovedFromList)
            {
                AllRoomsList.Remove(updatedRoom);
                //Debug.Log("removed from list ");
                RenderRoomList();
                continue;
            }
           
            RoomInfo existingRoom = AllRoomsList.Find(x => x.Name.Equals(updatedRoom.Name)); //foreach room, check to see it it already exists & store it in existingRoom
            
            if (existingRoom == null)
            {   //room was not found, so add it to the list
                //Debug.Log("that room did not exist");
                
                    AllRoomsList.Add(updatedRoom); //Existing room does not exist, so add to list of all rooms
                                                   //Debug.Log(updatedRoom.PlayerCount + "total palyer count just added");
                    if (updatedRoom.PlayerCount == 0)
                    { //it should be done automatically by photon, this code is not reached anymore
                        AllRoomsList.Remove(updatedRoom);
                        Debug.Log("removed empty room");
                        updatedRoom.RemovedFromList = true;
                    }
                    else if (updatedRoom.RemovedFromList)
                    {
                        AllRoomsList.Remove(existingRoom);
                    }
                    else if (updatedRoom.PlayerCount == 0)
                    {
                        Debug.Log("player count was 0");
                        AllRoomsList.Remove(existingRoom);
                    }
                //Added all rooms to the list, now render them to the screen'
                Debug.Log("render from roomListingMenu.cs");
                RenderRoomList();
            }else
            {
                Debug.Log("room found, re-render roomListingMenu.cs");
                RenderRoomList();
            }


            void RenderRoomList()
            {
                foreach (RoomInfo roomInfo in AllRoomsList)
                {
                    Debug.Log("from RLM.cs INIT Render stats: " + roomInfo.Name + "count: " + roomInfo.PlayerCount);
                }
                    RemoveRoomList(); //_listings holds current rooms, destroy all -> rebuild list -> render
                foreach (RoomInfo roomInfo in AllRoomsList)
                {
                   
                    Debug.Log("from RLM.cs name: " + roomInfo.Name + "count: " + roomInfo.PlayerCount);
                    RoomListItem roomListItem = Instantiate(_roomListItemPrefab, findRoomPanel).GetComponent<RoomListItem>();
                    roomListItem.Setup(roomInfo);//, roomInfo.Name, roomInfo.cu); //!
                    _listings.Add(roomListItem);
                }
            }
            void RemoveRoomList()
            {
                foreach (RoomListItem roomListItem in _listings)
                {
                    Destroy(roomListItem.gameObject);
                }
                _listings.Clear();
            }
        }
    }
}