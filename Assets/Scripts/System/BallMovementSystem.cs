using System.Collections.Generic;
using Model;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace System
{
    internal class BallMovementSystem
    {
        private readonly InputSystem _input;
        private readonly Ball _ball;
        private readonly Camera _camera;

        public BallMovementSystem(InputSystem input, Ball ball, Camera camera)
        {
            input.AddListener(Move);
            _input = input;
            _ball = ball;
            _camera = camera;
        }

        private void Move(InputAction.CallbackContext callback)
        {
            var position = _input.Position.ReadValue<Vector2>();
            var origin = _camera.ScreenPointToRay(position);

            Physics.Raycast(origin, out RaycastHit hit);
            var ground = hit.collider.GetComponentInParent<Ground>();

            if (ground == null)
                return;
            if (!ground.Raised)
                return;
            
            List<Vector2> shifteds = new List<Vector2>();
            ground.SwapWaveAsync(shifteds);
            
            _ball.MoveToTargetAsync(ground.transform.position);
        }
    }
}