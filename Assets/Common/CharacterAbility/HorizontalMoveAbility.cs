using UnityEngine;

namespace Common.CharacterAbility
{
    public class HorizontalMoveAbility : AbilityBase
    {
        public float moveSpeed;
        public float accelerationTimeGrounded;
        public float accelerationTimeAirborne;
        private float _velocityXSmoothing;
        private float vx;

        public override void HandleInput()
        {
            float targetVelocityX = _inputManager.HorizontalAxisInput * moveSpeed;
            
            vx = _controller.Velocity.x;
            vx = Mathf.SmoothDamp(vx, targetVelocityX, ref _velocityXSmoothing,
                (_controller.IsCollideWithBelow()) ? accelerationTimeGrounded : accelerationTimeAirborne);
            
            _controller.Velocity.x = vx;
        }
    }
}
