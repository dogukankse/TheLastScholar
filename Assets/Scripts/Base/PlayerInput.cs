using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Base
{
    public class PlayerInput : ScriptableObject
    {
        public Vector2 MovementVec => _movementVec;

        public Vector2 MousePos => _mousePos;

        public bool AllowDash { get; set; }
        public Vector2 LookDir { get; set; }

        public Action Fire;
        public Action Absorb;

        private const float DoubleTabTime = .2f;


        private Vector2 _movementVec;
        private Vector2 _mousePos;
        private Dictionary<int, float> _lastClick;
        private Camera _camera;

        private void Awake()
        {
            _lastClick = new Dictionary<int, float>
            {
                {0, 0},
                {1, 0},
                {2, 0},
                {3, 0},
                {4, 0}, //left mouse click
            };
            _camera = Camera.main;
        }

        public void DashInput()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                float deltaClickTime = Time.time - _lastClick[0];
                if (deltaClickTime <= DoubleTabTime)
                    AllowDash = true;

                _lastClick[0] = Time.time;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                float deltaClickTime = Time.time - _lastClick[1];
                if (deltaClickTime <= DoubleTabTime)
                    AllowDash = true;

                _lastClick[1] = Time.time;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                float deltaClickTime = Time.time - _lastClick[2];
                if (deltaClickTime <= DoubleTabTime)
                    AllowDash = true;

                _lastClick[2] = Time.time;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                float deltaClickTime = Time.time - _lastClick[3];
                if (deltaClickTime <= DoubleTabTime)
                    AllowDash = true;

                _lastClick[3] = Time.time;
            }
        }

        public bool AbsorbInput()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Absorb();
                return true;
            }

            return false;
        }

        public void MouseInput()
        {
            _mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);


            if (Input.GetMouseButton(0))
            {
                float deltaClickTime = Time.time - _lastClick[4];
                if (deltaClickTime > GameManager.Instance.PlayerFireRate)
                {
                    Fire();
                    _lastClick[4] = Time.time;
                }
            }
        }

        public void MovementInput()
        {
            _movementVec = new Vector2(0, 0);

            if (Input.GetKey(KeyCode.W))
            {
                _movementVec.y = +1f;
            }

            if (Input.GetKey(KeyCode.S))
            {
                _movementVec.y = -1f;
            }


            if (Input.GetKey(KeyCode.A))
            {
                _movementVec.x = -1f;
            }


            if (Input.GetKey(KeyCode.D))
            {
                _movementVec.x = +1f;
            }

            _movementVec.Normalize();
        }
    }
}