using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Demo.Scripts.Launcher
{
    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private Constants

        private const string PlayerNamePrefKey = "PlayerName";
   
        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            string defaultName = string.Empty;
            InputField inputField = this.GetComponent<InputField>();
            if (inputField != null)
            {
                PlayerPrefs.HasKey(PlayerNamePrefKey);
                defaultName = PlayerPrefs.GetString(PlayerNamePrefKey);
                inputField.text = defaultName;
            }

            PhotonNetwork.NickName = defaultName;
        }

        #endregion

        #region Public Methods

        public void SetPlayerName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player name is null or empty");
                return;
            }

            PhotonNetwork.NickName = value;
      
            PlayerPrefs.SetString(PlayerNamePrefKey, value);
        }

        #endregion
    }
}

