using Common.Core;
using Demo.Scripts.Manager;
using UnityEngine;

namespace Common.CharacterAbility
{
    public class JumpAbility : AbilityBase
    {
        private float _jumpVelocity;
        public override void InitWithOwnerCharacter(Character owner)
        {
            base.InitWithOwnerCharacter(owner);
            _jumpVelocity = _controller.JumpVelocity;
        }

        public override void HandleInput()
        {
            VirtualInputState jumpInputState = _inputManager.CurrentJumpInputState;
            if (_controller.IsCollideWithBelow() && jumpInputState.Equals(VirtualInputState.Up))
            {
                Debug.Log("JumpAbility.HandleInput called");
                _controller.Velocity.y = _jumpVelocity;
            }
            _inputManager.ResetJumpInput();
        }
    }
}

