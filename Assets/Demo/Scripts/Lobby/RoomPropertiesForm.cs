using System;
using TMPro;
using UnityEngine;
using ExitGames.Client.Photon;

namespace Demo.Scripts.Lobby
{
    public class RoomPropertiesForm : MonoBehaviour
    {
        private string _roomTitle;
        private string _hostName;
    
        [SerializeField] private TMP_InputField titleInputField;

        public Hashtable GetProperties()
        {
            Hashtable properties = new ExitGames.Client.Photon.Hashtable();
            try
            {
                _roomTitle = titleInputField.textComponent.text;
                _hostName = PlayerPrefs.GetString("PlayerName");

                properties.Add(RoomPropertyKeys.ROOMTITLE, _roomTitle);
                properties.Add(RoomPropertyKeys.HOSTNAME, _hostName);
            }
            catch (NullReferenceException exception)
            {
                Debug.LogError(exception.Message);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
            }
        
            return properties;
        }

    }
}

