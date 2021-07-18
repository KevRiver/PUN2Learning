using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Demo.Scripts.Room
{
    public class RoomUIController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject roomPanel;
        [SerializeField] private TMP_Text roomGuid;
        [SerializeField] private TMP_Text roomTitle;
        [SerializeField] private Button leaveButton;
        [SerializeField] private Button startGameButton;
        [SerializeField] private Transform playerListView;
        [SerializeField] private GameObject playerListItemPrefab;
        
        private List<PlayerListItem> _items;
        
        private string _arenaName = "Arena";
        public string ArenaName => _arenaName;
        
        private void Start()
        {
            _items = new List<PlayerListItem>();
            
            HideRoomPanel();
        }
        
        public override void OnJoinedRoom()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            InitRoomPanel();
            ShowRoomPanel();
        }
            
        public override void OnPlayerEnteredRoom(Player enteredPlayer)
        {
            PlayerListItemModel model = 
                new PlayerListItemModel(enteredPlayer.ActorNumber, 
                    enteredPlayer.NickName,
                    enteredPlayer.IsMasterClient);
            
            AttachPlayerListItemToView(model);
        }
            
        public override void OnPlayerLeftRoom(Player leftPlayer)
        {
            int leftPlayerNumber = leftPlayer.ActorNumber;
            int i = _items.FindIndex(playerListItem => playerListItem.PlayerNumber == leftPlayerNumber);
            if (i > -1) // index has found
            {
                Destroy(_items[i].gameObject);
                _items.RemoveAt(i);
            }
        }
            
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            int newMasterClientPlayerNumber = newMasterClient.ActorNumber;
            int i = _items.FindIndex(PlayerListItem => PlayerListItem.PlayerNumber == newMasterClientPlayerNumber);
            if (i > -1)
            {
                _items[i].ActivateHostIcon(true);
            }
            UpdateRoomHost(newMasterClient);
            ActivatePlayButton(PhotonNetwork.IsMasterClient);
        }
            
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.LogError("OnJoinRoomFailed called. returnCode: " + returnCode + ". " + message);
        }
        protected void InitRoomPanel()
        {
            SetRoomGUID(PhotonNetwork.CurrentRoom.Name);
            SetRoomTitle();
            SetPlayerListView();
            ActivatePlayButton(PhotonNetwork.IsMasterClient);
            SetLeaveButtonOnClickCallback();
            SetStartGameButtonOnClickCallback();
        }

        private void SetRoomGUID(string guid)
        {
            try
            {
                roomGuid.text = guid;
            }
            catch (NullReferenceException exception)
            {
                Debug.LogError(exception.Message);
            }
        }
        
        private void SetRoomTitle()
        {
            object title;
            try
            {
                PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RoomPropertyKeys.ROOMTITLE, out title);
                roomTitle.text = title.ToString();
            }
            catch (NullReferenceException exception)
            {
                Debug.LogError(exception.Message);
            }
        }

        #region struct PlayerListItemModel

        private struct PlayerListItemModel
        {
            public int playerNumber;
            public string playerNickname;
            public bool isHostPlayer;

            public PlayerListItemModel(int number, string name, bool isHost)
            {
                playerNumber = number;
                playerNickname = name;
                isHostPlayer = isHost;
            }
        }

        #endregion
        
        private void SetPlayerListView()
        {
            List<PlayerListItemModel> models = GetCurrentRoomPlayerDataModels();
            foreach (var model in models)
            {
                AttachPlayerListItemToView(model);
            }
        }
        
        private List<PlayerListItemModel> GetCurrentRoomPlayerDataModels()
        {
            var playerTable = PhotonNetwork.CurrentRoom.Players;
            List<PlayerListItemModel> models = new List<PlayerListItemModel>();
            foreach (var entitiy in playerTable)
            {
                int actorNumber = entitiy.Key;
                string playerNickname = entitiy.Value.NickName;
                bool isHostPlayer = PhotonNetwork.CurrentRoom.MasterClientId == actorNumber;
                
                models.Add(new PlayerListItemModel(actorNumber, playerNickname, isHostPlayer));
            }
            
            models.Sort((a, b) => a.playerNumber.CompareTo(b.playerNumber));

            return models;
        }
        
        private void AttachPlayerListItemToView(PlayerListItemModel model)
        {
            GameObject item = Instantiate(playerListItemPrefab, playerListView);
            PlayerListItem playerListItem = item.GetComponent<PlayerListItem>();
            if (playerListItem == null) throw new NullReferenceException("There's no PlayerListItem Component");
            
            playerListItem.SetPlayerNumber(model.playerNumber);
            playerListItem.SetPlayerNickname(model.playerNickname);
            playerListItem.ActivateHostIcon(model.isHostPlayer);

            _items.Add(playerListItem); // ambiguos line
        }
        
        private void ActivatePlayButton(bool active)
        {
            startGameButton.gameObject.SetActive(active);
        }

        private void SetLeaveButtonOnClickCallback()
        {
            leaveButton.onClick.AddListener((() =>
            {
                if (!PhotonNetwork.InRoom) return;
                ProceedLeaveProcedure();
            }));
        }

        private void ProceedLeaveProcedure()
        {
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.LeaveRoom();
            HideRoomPanel();
            DetachAllPlayerListItemsFromView();
        }

        private void DetachAllPlayerListItemsFromView()
        {
            foreach (var item in _items)
            {
                Destroy(item.gameObject);
            }
            _items.Clear();
        }

        private void SetStartGameButtonOnClickCallback()
        {
            startGameButton.onClick.AddListener((() =>
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.LoadLevel(_arenaName);
            }));
        }
        
        private void UpdateRoomHost(Player newHost)
        {
            string newHostNickName = newHost.NickName;
            try
            {
                Hashtable newCustomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
                newCustomProperties[RoomPropertyKeys.HOSTNAME] = newHostNickName;
                PhotonNetwork.CurrentRoom.SetCustomProperties(newCustomProperties);
            }
            catch (KeyNotFoundException exception)
            {
                Debug.LogError(exception.Message);
            }
        }
        
        protected void HideRoomPanel()
        {
            roomPanel.SetActive(false);
        }
        
        protected void ShowRoomPanel()
        {
            roomPanel.SetActive(true);
        }
    }
}

