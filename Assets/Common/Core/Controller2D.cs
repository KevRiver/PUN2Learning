#define DEBUG_CONTROLLER2D
using System;
using Tools;
using UnityEngine;

namespace Common.Core
{
	#region struct RaycastOrigins

	struct RaycastOrigins
	{
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

	#endregion
	
	#region struct CollisionInfo
	public struct CollisionInfo
	{
		public bool above, below;
		public bool left, right;

		public void Reset()
		{
			above = below = false;
			left = right = false;
		}
	}
	#endregion
	
	[RequireComponent(typeof(BoxCollider2D))]
	public class Controller2D : MonoBehaviour
	{
		[Header("Gravity Calculation Factors")]
		public float jumpHeight = 10f;
		public float timeToReachApex = 1.5f;
		
		[Header("Valid Collision LayerMask")]
		public LayerMask collisionMask;

		const float skinWidth = .015f;
		private const float rayOffset = 0.05f;
		public int horizontalRayCount = 4;
		public int verticalRayCount = 4;

		float horizontalRaySpacing;
		float verticalRaySpacing;

		BoxCollider2D _collider;
		RaycastOrigins raycastOrigins;
		private CollisionInfo _isCollideWith;
		
		private CharacterMovementState _movement;
		private CharacterConditionState _condition;
        
		private StateMachine<CharacterMovementState> _movementState;
		public CharacterMovementState CurrentMovementState => _movementState.currentState;
        
		private StateMachine<CharacterConditionState> _conditionState;
		public CharacterConditionState CurrentConditionState => _conditionState.currentState;
		
		[Header("Velocity")]
		public Vector3 Velocity = Vector2.zero;
		[Header("Jump Velocity")]
		public float JumpVelocity;
		
		private float _gravity;
		private bool _gravityActive = true;
		private float _gravityApplied = 0f;

		private const float SPEED_THRESHOLD = 0.001f;
		
		public void Init()
		{
			_collider = GetComponent<BoxCollider2D>();
			
			_movementState=new StateMachine<CharacterMovementState>();
			_movementState.ChangeState(CharacterMovementState.Idle);
			
			_conditionState=new StateMachine<CharacterConditionState>();
			_conditionState.ChangeState(CharacterConditionState.Idle);
			
			CalculateRaySpacing();
			
			_gravity = CalculateGravity(jumpHeight, timeToReachApex);
			JumpVelocity = CalculateJumpVelocity(_gravity, timeToReachApex);
		}
		
		private void CalculateRaySpacing()
		{
			Bounds bounds = _collider.bounds;
			bounds.Expand(skinWidth * -2);

			horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
			verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

			horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
			verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
		}

		/// <summary>
		/// Gravity 값을 계산합니다
		/// </summary>
		/// <param name="jumpHeight">높이</param>
		/// <param name="timeToJumpApex">최고점에 도달하는 시간</param>
		/// <returns></returns>
		private float CalculateGravity(float jumpHeight,float timeToJumpApex)
		{
			float g = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
			return g;
		}

		/// <summary>
		/// JumpVelocity 값을 계산합니다
		/// </summary>
		/// <param name="gravity">중력</param>
		/// <param name="timeToJumpApex">최고점에 도달하는 시간</param>
		/// <returns></returns>
		public float CalculateJumpVelocity(float gravity, float timeToJumpApex)
		{
			float jv = Mathf.Abs(gravity) * timeToJumpApex;
			return jv;
		}
		
		public void ProcessFrame()
		{
			Move(Velocity * Time.deltaTime);
			
			ProcessCharacterMovementState();
		}
		
		public void PreprocessMovement()
		{
			if (IsCollideWithBelow() || IsCollideWithAbove())
			{
				Velocity.y = 0;
			}
		}
		
		public void ApplyGravity()
		{
			_gravityApplied = _gravity * Time.deltaTime;
			if (IsCollideWithBelow() && !_gravityActive) _gravityActive = true;
			if(_gravityActive)
				Velocity.y += _gravityApplied;
		}

		private void Move(Vector3 velocity)
		{
			Physics2D.SyncTransforms();
			UpdateRaycastOrigins();
			_isCollideWith.Reset();

			if (Math.Abs(velocity.x) > 0)
			{
				CheckHorizontalCollisions(ref velocity);
			}

			if (Math.Abs(velocity.y) > _gravityApplied)
			{
				CheckVerticalCollisions(ref velocity);
			}
			
			transform.Translate(velocity);
		}
		
		private void UpdateRaycastOrigins()
		{
			Bounds bounds = _collider.bounds;
			bounds.Expand(skinWidth * -2);

			raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
			raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
			raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
			raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
		}
		
		private void CheckHorizontalCollisions(ref Vector3 velocity)
		{
			float directionX = Mathf.Sign(velocity.x);
			float rayLength = Mathf.Abs(velocity.x) + skinWidth;

			for (int i = 0; i < horizontalRayCount; i++)
			{
				Vector2 rayOrigin = (directionX < 0) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
				rayOrigin += Vector2.up * (horizontalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
#if DEBUG_CONTROLLER2D && UNITY_EDITOR
				Debug.DrawRay(rayOrigin, rayLength * directionX * Vector2.right, Color.green);
#endif
				if (hit)
				{
					velocity.x = (hit.distance - skinWidth) * directionX;
					rayLength = hit.distance + rayOffset;

					_isCollideWith.left = directionX < 0;
					_isCollideWith.right = directionX > 0;
				}
			}
		}
		
		private void CheckVerticalCollisions(ref Vector3 velocity)
		{
			float directionY = Mathf.Sign(velocity.y);
			float rayLength = Mathf.Abs(velocity.y) + skinWidth;

			for (int i = 0; i < verticalRayCount; i++)
			{
				Vector2 rayOrigin = (directionY < 0) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
				rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
#if DEBUG_CONTROLLER2D && UNITY_EDITOR
				Debug.DrawRay(rayOrigin, dir: rayLength * directionY * Vector2.up, Color.red);
#endif
				if (hit)
				{
					velocity.y = (hit.distance - skinWidth) * directionY;
					rayLength = hit.distance;

					_isCollideWith.below = directionY < 0;
					_isCollideWith.above = directionY > 0;
				}
			}
		}

		private void ProcessCharacterMovementState()
		{
			Vector3 v = Velocity;
            
			if (IsCollideWithBelow())
			{
				_movementState.ChangeState(CharacterMovementState.Idle,true);
				return;
			}
            
			if (v.y > 0)
			{
				_movementState.ChangeState(CharacterMovementState.Jump,true);
				return;
			}
            
			if (v.y < 0)
			{
				_movementState.ChangeState(CharacterMovementState.Fall,true);
				return;
			}
		}

		public bool IsCollideWithBelow()
		{
			return _isCollideWith.below;
		}

		public bool IsCollideWithAbove()
		{
			return _isCollideWith.above;
		}

		public bool IsCollideWithLeft()
		{
			return _isCollideWith.left;
		}

		public bool IsCollideWithRight()
		{
			return _isCollideWith.right;
		}
	}
}

