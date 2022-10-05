
         /*   
            for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            {
                Destroy(_listings[i].gameObject);
                _listings.RemoveAt(i);
                Debug.Log("removed from list");
                *//*int index = _listings.FindIndex(x => x.info.Name == info.Name);
                if (index != -1)
                {
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }*//*
            }
            else
            {
                Debug.Log("Instantiated a room" + roomList[i].Name);
                RoomListItem listing = Instantiate(_roomListItemPrefab, _content);
                if (listing != null)
                {
                    Debug.Log("populating with room");
                    listing.Setup(roomList[i]);
                    _listings.Add(listing);
                }
            }
        */




using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListingsMenuNew : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _content;
    [SerializeField] private RoomListItem _roomListItemPrefab;

    private List<RoomListItem> _listings = new List<RoomListItem>();
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("new on room list update");
        //foreach (RoomInfo info in roomList)
        //{
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            {
                Destroy(_listings[i].gameObject);
                _listings.RemoveAt(i);
                Debug.Log("removed room " + _listings[i].text + " from the list");
            }
            else
            {
                Debug.Log("Instantiated a room" + roomList[i].Name);
                RoomListItem listing = Instantiate(_roomListItemPrefab, _content);
                if (listing != null)
                {
                    Debug.Log("populating with room");
                    listing.Setup(roomList[i]);
                    _listings.Add(listing);
                }
            }
        }
        int count = roomList.Count;
        Debug.Log("count is : " + count);
        for (int i = 1; i < count; i++)
        {
            for (int j = 1; j < count; j++)
            {
                if (roomList[i] == roomList[j])
                {
                    Debug.Log(i);
                    _listings.RemoveAt(i);
                }
            }
        }

    }
}

