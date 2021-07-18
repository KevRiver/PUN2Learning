using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Demo.Scripts.Manager
{
    public enum VirtualInputState
    {
        None,
        Down,
        Pressed,
        Up
    }

    public class InputManager : Singleton<InputManager>
    {
        private const int LEFT_BUTTON = 0;
        private const int RIGHT_BUTTON = 1;
        private const int MIDDLE_BUTTON = 2;
        
        private float _horizontalAxisInput;
        public float HorizontalAxisInput => _horizontalAxisInput;

        private StateMachine<VirtualInputState> _aimInputState;
        public VirtualInputState CurrentAimInputState => _aimInputState.currentState;

        private Vector3 _mousePositionWorld = Vector3.zero;
        public Vector3 MousePositionWorld => _mousePositionWorld;

        private StateMachine<VirtualInputState> _jumpInputState;
        public VirtualInputState CurrentJumpInputState => _jumpInputState.currentState;

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        private void Init()
        {
            _aimInputState = new StateMachine<VirtualInputState>();
            _aimInputState.ChangeState(VirtualInputState.None);

            _jumpInputState = new StateMachine<VirtualInputState>();
            _jumpInputState.ChangeState(VirtualInputState.None);
        }

        private void Update()
        {
            GetHorizontalAxis();
            GetMouseInput(LEFT_BUTTON);
            GetKeyInput();
        }

        private void GetHorizontalAxis()
        {
            _horizontalAxisInput = Input.GetAxis("Horizontal");
        }

        private void GetMouseInput(int button)
        {
            switch (button)
            {
                case LEFT_BUTTON:
                {
                    ProcessMouseLeftButtonInput();
                }
                    break;
                case RIGHT_BUTTON:
                    break;
                case MIDDLE_BUTTON:
                    break;
                default:
                    break;
            }
        }

        private void GetKeyInput()
        {
            VirtualInputState newState = VirtualInputState.None;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                newState = VirtualInputState.Down;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                newState = VirtualInputState.Pressed;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                newState = VirtualInputState.Up;
            }
            
            _jumpInputState.ChangeState(newState);
        }

        private void ProcessMouseLeftButtonInput()
        {
            if (Input.GetMouseButtonDown(LEFT_BUTTON))
            {
                _aimInputState.ChangeState(VirtualInputState.Down);
            }
            else if (Input.GetMouseButton(LEFT_BUTTON))
            {
                SetMousePositionWorld2D();
                _aimInputState.ChangeState(VirtualInputState.Pressed);
            }
            else if(Input.GetMouseButtonUp(LEFT_BUTTON))
            {
                _aimInputState.ChangeState(VirtualInputState.Up);
            }
        }
        
        private void SetMousePositionWorld2D()
        {
            _mousePositionWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _mousePositionWorld.z = 0;
        }

        public void ResetAimInput()
        {
            _aimInputState.ChangeState(VirtualInputState.None);
            _mousePositionWorld = Vector2.zero;
        }

        public void ResetJumpInput()
        {
            _jumpInputState.ChangeState(VirtualInputState.None);
        }
    }
}

