using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Demo.Scripts.Room
{
    public class PlayerListItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text playerNumber;
        [SerializeField] private TMP_Text playerNickname;
        [SerializeField] private Image hostIcon;

        private int _playerNumber;
        public int PlayerNumber
        {
            get { return _playerNumber;}
        }
    
        private string _playerNickname;
        public string PlayerNickname
        {
            get { return _playerNickname;}
        }
    
        private bool _playerIsHost;
        public bool PlayerIsHost
        {
            get { return _playerIsHost;}
        }

        public void SetPlayerNumber(int actorNumber)
        {
            _playerNumber = actorNumber;
            playerNumber.text = actorNumber.ToString();
        }

        public void SetPlayerNickname(string nickname)
        {
            _playerNickname = nickname;
            playerNickname.text = nickname;
        }

        public void ActivateHostIcon(bool isMasterClient)
        {
            _playerIsHost = isMasterClient;
            hostIcon.gameObject.SetActive(isMasterClient);
        }
    }
}

