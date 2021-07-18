using System;
using Demo.ScriptableObejct;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Common.Core
{
    public class Health : MonoBehaviour, IPunObservable
    {
        [SerializeField] private GameEvent onPlayerDied;

        private Character _owner;
        public Character Owner => _owner;
        
        private PlayerUIController _uiController;

        public int maxHealth = 100;

        private int _currentHealth = 100;
        public int CurrentHealth => _currentHealth;

        private bool _isInitialized = false;

        public void InitWithOwnerCharacter(Character owner)
        {
            _uiController = GetComponent<PlayerUIController>();
            if (_uiController is null)
                throw new NullReferenceException("Health.InitWithOwnerCharacter() failed. PlayerUIController is null");
            
            _owner = owner;
            
            _currentHealth = maxHealth;

            _isInitialized = true;
        }
    
        public void Damage(int amount)
        {
            if (!_owner.isLocalPlayer)
                return;

            _currentHealth -= amount;
            UpdateHealthUI();
            if(IsPlayerDead())
                RaisePlayerDiedEvent();
        }
    
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (!_isInitialized) return;
            
            if (stream.IsWriting)
            {
                stream.SendNext(_currentHealth);
            }
            else
            {
                _currentHealth = (int)stream.ReceiveNext();
                UpdateHealthUI();
                if (IsPlayerDead())
                    RaisePlayerDiedEvent();

            }
        }
    
        private void UpdateHealthUI()
        {
            _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);

            _uiController.UpdateHealthText(_currentHealth.ToString());
        }
    
        private bool IsPlayerDead()
        {
            return _currentHealth <= 0;
        }

        private void RaisePlayerDiedEvent()
        {
            if (onPlayerDied is null) return;
            
            onPlayerDied.Raise(_owner.playerNickname);
        }
    }
}
