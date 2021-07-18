#define DEBUG_CHARACTER

using System;
using System.Collections.Generic;
using System.Linq;
using Common.CharacterAbility;
using Photon.Pun;
using UnityEngine;

namespace Common.Core
{
    public enum CharacterMovementState
    {
        Idle,
        Jump,
        InApex,
        Fall,
        Drop
    }

    public enum CharacterConditionState
    {
        Dead,
        Idle,
        Invincible
    }
    
    [RequireComponent(typeof(Controller2D))]
    public class Character : MonoBehaviour
    {
        [SerializeField] 
        private Transform characterModel;
        
        private Controller2D _controller;

        private Health _health;

        private PlayerUIController _uiController;
        
        private List<AbilityBase> _abilities = new List<AbilityBase>();

        [HideInInspector]
        public PhotonView photonView;

        [HideInInspector] 
        public bool isLocalPlayer = false;

        [HideInInspector]
        public string playerNickname = "";

        void Start()
        {
            try
            {
                Init();
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

        void Init()
        {
            _controller = GetComponent<Controller2D>();
            if (_controller is null) throw new NullReferenceException("Controller2D component is null");

            _health = GetComponent<Health>();
            if (_health is null) throw new NullReferenceException("Health component is null");

            _uiController = GetComponent<PlayerUIController>();
            if (_uiController is null) throw new NullReferenceException("PlayerUIController component is null");
            
            photonView = GetComponent<PhotonView>();
            if (photonView is null) throw new NullReferenceException("PhotonView component is null");

            playerNickname = photonView.Owner.NickName;
            
            if (photonView.IsMine)
            {
                gameObject.layer = LayerMask.NameToLayer("LocalPlayer");
                isLocalPlayer = true;
            }
            else
            {
                characterModel.GetComponent<SpriteRenderer>().color = Color.red;
            }

            _controller.Init();
            _health.InitWithOwnerCharacter(this);
            _uiController.InitWithOwnerCharacter(this);
            
            _abilities = GetComponents<AbilityBase>().ToList();
            for (int i = 0; i < _abilities.Count; i++)
            {
                _abilities[i].InitWithOwnerCharacter(this);
            }
        }

        void Update()
        {
            if (!IsLocalPlayer()) return;

            PreProcessFrame();

            HandleInputs();
            
            ProcessFrame();
        }

        private bool IsLocalPlayer()
        {
            return photonView.IsMine;
        }

        private void PreProcessFrame()
        {
            _controller.PreprocessMovement();
            
            _controller.ApplyGravity();
        }

        private void HandleInputs()
        {
            for (int i = 0; i < _abilities.Count; i++)
            {
                _abilities[i].HandleInput();
            }
        }

        private void ProcessFrame()
        {
            _controller.ProcessFrame();
        }
        
        public Controller2D GetMovementController()
        {
            return _controller;
        }

        public Health GetHealthComponent()
        {
            return _health;
        }
    }
}


    

    
