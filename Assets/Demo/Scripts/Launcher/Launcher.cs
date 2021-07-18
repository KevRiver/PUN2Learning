using System;
using System.Runtime.CompilerServices;
using Demo.ScriptableObejct;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace Demo.Scripts.Launcher
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serialize Field
        
        [SerializeField] private NetworkEvent tryToConnect;
        [SerializeField] private NetworkEvent connectionSucceeded;
        [SerializeField] private NetworkEvent connectionFailed;
        
        #endregion

        #region Private Fields

        private string _gameVersion = "0.0.0";

        #endregion

        #region MonoBehaviour Callbacks

        

        #endregion

        #region Public Methods

        public void Connect()
        {
            try
            {
                tryToConnect.Raise();
            }
            catch (NullReferenceException exception)
            {
                Debug.LogWarning(exception.Message);
            }
            finally
            {
                PhotonNetwork.GameVersion = _gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster called");
            try
            {
                connectionSucceeded.Raise();
                SceneManager.LoadScene(1, LoadSceneMode.Single);
            }
            catch (NullReferenceException exception)
            {
                Debug.LogWarning(exception.Message);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
            }
        }
        
        public override void OnDisconnected(DisconnectCause cause)
        {
            try
            {
                connectionFailed.Raise();
            }
            catch (NullReferenceException exception)
            {
                Debug.LogWarning(exception.Message);
            }

            Debug.LogWarningFormat("Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        #endregion
    }
}
