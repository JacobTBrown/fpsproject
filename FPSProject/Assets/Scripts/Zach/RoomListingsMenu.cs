using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListingsMenu : MonoBehaviourPunCallbacks
{

    [SerializeField] private Transform _content;
    [SerializeField] private RoomListItem _roomListItemPrefab;

    List<RoomInfo> fullRoomList = new List<RoomInfo>();
    private List<RoomListItem> _listings = new List<RoomListItem>();
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //_listings = roomList;
        Debug.Log("new on room list update");
        foreach (RoomInfo updatedRoom in roomList)
        {
            if (updatedRoom.RemovedFromList)
            {
                fullRoomList.Remove(updatedRoom);
                Debug.Log("removed from list ");
                RenderRoomList();
                continue;
            }
            RoomInfo existingRoom = fullRoomList.Find(x => x.Name.Equals(updatedRoom.Name)); //foreach room, check to see it it already exists & store it in existingRoom
            
            if (existingRoom == null)
            {
                Debug.Log("esiting room did not exist");

                fullRoomList.Add(updatedRoom); //Existing room does not exist, so add to list of full rooms
                Debug.Log(updatedRoom.PlayerCount + "total palyer count just added");
                if (updatedRoom.PlayerCount == 0)
                {
                    fullRoomList.Remove(updatedRoom);
                    Debug.Log("removed that");
                    updatedRoom.RemovedFromList = true;
                }
                else if (updatedRoom.RemovedFromList)
                {
                    fullRoomList.Remove(existingRoom);
                }
                else if (updatedRoom.PlayerCount == 1)
                {
                    Debug.Log("player count was 1");
                    fullRoomList.Remove(existingRoom);
                }
                else if (updatedRoom.PlayerCount == 0)
                {
                    Debug.Log("player count was 0");
                    fullRoomList.Remove(existingRoom);
                }

                RenderRoomList();
            }


            void RenderRoomList()
            {
                RemoveRoomList();
                foreach (RoomInfo roomInfo in fullRoomList)
                {
                    RoomListItem roomListItem = Instantiate(_roomListItemPrefab, _content).GetComponent<RoomListItem>();
                    roomListItem.Setup(roomInfo);
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