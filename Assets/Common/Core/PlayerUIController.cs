using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Core
{
    public class PlayerUIController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _playerNicknameText;
        [SerializeField] private TMP_Text _healthText;

        private Character _owner;

        public void InitWithOwnerCharacter(Character owner)
        {
            _owner = owner;
            
            _playerNicknameText.text = _owner.playerNickname.ToUpper();
            _healthText.text = _owner.GetHealthComponent().CurrentHealth.ToString();
        }
        

        public void UpdatePlayerNicknameText(string text)
        {
            _playerNicknameText.text = text;
        }

        public void UpdateHealthText(string text)
        {
            _healthText.text = text;
        }
    }
}
