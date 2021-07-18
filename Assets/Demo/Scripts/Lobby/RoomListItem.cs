using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Demo.Scripts.Lobby
{
    public class RoomListItem : MonoBehaviour
    {

        [SerializeField] private TMP_Text roomTitle;
        [SerializeField] private TMP_Text roomHostName;
        [SerializeField] private TMP_Text joinedPlayerCount;
        [SerializeField] private Button joinRoomButton;
        
        private RoomInfo _roomInfo;
        public RoomInfo RoomInfo => _roomInfo;

        public void SetRoomInfoAndInitialize(RoomInfo roomInfo)
        {
            _roomInfo = roomInfo;
            
            InitializeListItem();
        }

        #region Private Methods
        
        private void InitializeListItem()
        {
            SetTitle();
            SetHostName();
            SetJoinedPlayerCount();
            SetJoinButtonOnClickMethod();
        }

        private void SetTitle()
        {
            object title = string.Empty;
            try
            {
                bool isKeyExist = _roomInfo.CustomProperties.TryGetValue(RoomPropertyKeys.ROOMTITLE, out title);
                
                if (!isKeyExist) throw new KeyNotFoundException("Key doesn't exist");

                if (title == null) throw new NullReferenceException("Value is null");
            }
            catch (KeyNotFoundException exception)
            {
                Debug.LogWarning(exception.Message);
                title = _roomInfo.Name;
            }
            catch (NullReferenceException exception)
            {
                Debug.LogWarning(exception.Message);
                title = _roomInfo.Name;
            }

            try
            {
                roomTitle.SetText(title.ToString());
            }
            catch (NullReferenceException exception)
            {
                Debug.LogError(exception.Message);
            }
        }

        private void SetHostName()
        {
            object hostName = string.Empty;
            try
            {
                bool isKeyExist = _roomInfo.CustomProperties.TryGetValue(RoomPropertyKeys.HOSTNAME, out hostName);

                if (!isKeyExist) throw new KeyNotFoundException();

                if (hostName == null) throw new NullReferenceException();
            }
            catch (KeyNotFoundException exception)
            {
                Debug.LogWarning(exception.Message);
                hostName = _roomInfo.masterClientId.ToString();
            }
            catch (NullReferenceException exception)
            {
                Debug.LogWarning(exception.Message);
                hostName = _roomInfo.masterClientId.ToString();
            }

            try
            {
                roomHostName.SetText(hostName.ToString());
            }
            catch (NullReferenceException exception)
            {
                Debug.LogError(exception.Message);
            }
        }

        private void SetJoinedPlayerCount()
        {
            try
            {
                string status = _roomInfo.PlayerCount + "/" + _roomInfo.MaxPlayers;
                joinedPlayerCount.SetText(status);
            }
            catch (NullReferenceException exception)
            {
                Debug.LogError(exception.Message);
            }
        }
        
        private void SetJoinButtonOnClickMethod()
        {
            if (joinRoomButton == null) throw new NullReferenceException("Join Button is null");
            joinRoomButton.onClick.AddListener(() =>
            {
                PhotonNetwork.JoinRoom(_roomInfo.Name);
            });
        }
        
        #endregion
    }
}

