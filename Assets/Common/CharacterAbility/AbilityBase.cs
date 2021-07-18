using Common.Core;
using Demo.Scripts.Manager;
using UnityEngine;

namespace Common.CharacterAbility
{
    public class AbilityBase : MonoBehaviour
    {
        protected Character _owner;
        protected Controller2D _controller;
        protected InputManager _inputManager;
        
        public void SetOwner(Character owner)
        {
            _owner = owner;
        }

        public void SetController2D(Controller2D controller)
        {
            _controller = controller;
        }

        public void SetInputManager()
        {
            _inputManager = InputManager.Instance;
        }

        public virtual void InitWithOwnerCharacter(Character owner)
        {
            SetOwner(owner);
            SetController2D(owner.GetMovementController());
            SetInputManager();
        }

        public virtual void HandleInput()
        {
            
        }
    }
}

