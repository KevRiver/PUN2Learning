using System;
using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace Demo.Scripts.Lobby
{
    public class LobbyPunCallbackListener : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        try
        {
            CheckConnectionWithMasterServer();
            TryJoinLobby();
        }
        catch (Exception exception)
        {
            Debug.LogError(exception.Message);
        }
    }

    private void CheckConnectionWithMasterServer()
    {
        if (!PhotonNetwork.IsConnected) throw new Exception("there's no connection with Photon master server");
    }

    private void TryJoinLobby()
    {
        if (!PhotonNetwork.JoinLobby()) throw new Exception("JoinLobby failed");
    }

    #region MonoBehaviourPunCallbacks Methods
    
    public override void OnConnectedToMaster()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }
    
    public override void OnCreatedRoom()
    {
        Debug.Log("Room Creation succeeded");
    }
    
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Room Creation Failed. " + message);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Player joined room");
        GetRoomInfo();
    }
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogFormat("OnJoinRoomFailed called. code : {0}, {1}", returnCode, message);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("TestConnect: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    #endregion
    
    private void GetRoomInfo()
    {
        int masterClientActorNumber = PhotonNetwork.CurrentRoom.MasterClientId;
        Dictionary<int, Player> players = PhotonNetwork.CurrentRoom.Players;

        foreach (var player in players)
        {
            int actorNumber = player.Value.ActorNumber;
            string nickname = player.Value.NickName;
            if (actorNumber == masterClientActorNumber)
            {
                Debug.LogFormat("{0}. {1} is master client of the room", actorNumber, nickname);
            }
            else
            {
                Debug.LogFormat("{0}. {1}", actorNumber, nickname);
            }
        }
    }
}
}

