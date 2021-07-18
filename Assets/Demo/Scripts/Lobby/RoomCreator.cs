using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Demo.Scripts.Lobby
{
    public class RoomCreator : MonoBehaviour
    {
        private RoomOptions _roomOptions;

        [SerializeField] private RoomPropertiesForm customRoomProperties;
        [SerializeField] private Button createRoomButton;

        private void Start()
        {
            try
            {
                if (createRoomButton == null) throw new NullReferenceException("CreateRoomButton is null");
                createRoomButton.onClick.AddListener((RequestToCreateRoom));
            }
            catch (NullReferenceException exception)
            {
                Debug.LogError(exception.Message);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
            }
        }
        private void RequestToCreateRoom()
        {
            Debug.Log("RequsetToCreateRoom called");
            InitRoomOptions();
            if (!PhotonNetwork.CreateRoom(null, _roomOptions))
            {
                Debug.LogError("PhotonNetwork.CreateRoom failed");
            }
        }
        
        private void InitRoomOptions()
        {
            _roomOptions = new RoomOptions();
            _roomOptions.MaxPlayers = 2;
            _roomOptions.PlayerTtl = 0;
            _roomOptions.EmptyRoomTtl = 0;
            _roomOptions.CustomRoomProperties = customRoomProperties.GetProperties();
            _roomOptions.CustomRoomPropertiesForLobby =
                new string[] {RoomPropertyKeys.ROOMTITLE, RoomPropertyKeys.HOSTNAME};
        }
    }

}
