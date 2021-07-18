using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Demo.Scripts.Lobby
{
    public class RoomListLoader : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Transform content;
        [SerializeField] private GameObject roomListItemPrefab;

        private List<RoomListItem> _items;
        
        private void Awake()
        {
            _items = new List<RoomListItem>();
        }

        /// <summary>
        /// MonobehaviourPunCallbacks.OnRoomListUpdate implementation
        /// Remove, Update or Instantiate RoomListItem depending on RoomInfo
        /// </summary>
        /// <param name="roomList"></param>
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("OnRoomListUpdate called");
            foreach (RoomInfo info in roomList)
            {
                if (info.RemovedFromList)
                {
                    int i = _items.FindIndex(x => x.RoomInfo.Name == info.Name);
                    if (i != -1) // index has found
                    {
                        Destroy(_items[i].gameObject);
                        _items.RemoveAt(i);
                    }
                    continue;
                }
                
                int whichToUpdate = _items.FindIndex(item => item.RoomInfo.Name == info.Name);
                if (whichToUpdate > -1) // index has found
                {
                    UpdateRoomListItemFromRoomInfo(_items[whichToUpdate], info);
                }
                else
                {
                    InstantiateRoomListItemFromRoomInfo(info);
                }
            }
        }
        
        private void UpdateRoomListItemFromRoomInfo(RoomListItem item, RoomInfo info)
        {
            item.SetRoomInfoAndInitialize(info);
        }
        
        private void InstantiateRoomListItemFromRoomInfo(RoomInfo roomInfo)
        {
            try
            {
                if (roomListItemPrefab == null) throw new NullReferenceException("Prefab is null");
                
                GameObject item = Instantiate(roomListItemPrefab, content, false);

                RoomListItem roomListItem = item.GetComponent<RoomListItem>();
                if (roomListItem == null) throw new NullReferenceException("RoomListItem component should be attached");
                
                roomListItem.SetRoomInfoAndInitialize(roomInfo);
                _items.Add(roomListItem);
            }
            catch (NullReferenceException exception)
            {
                Debug.LogError(exception.Message);
            }
        }
    }
}

