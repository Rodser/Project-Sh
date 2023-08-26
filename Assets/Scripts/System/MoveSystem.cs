using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.InputSystem;

namespace System
{
    internal class MoveSystem
    {
        private readonly InputSystem _input;
        private readonly BallSystem _ballSystem;

        public MoveSystem(InputSystem input, BallSystem ballSystem)
        {
            input.Click.performed += Move;
            _input = input;
            _ballSystem = ballSystem;
        }

        private void Move(InputAction.CallbackContext callback)
        {
            var position = _input.Position.ReadValue<Vector2>();
            var origin = Camera.main.ScreenPointToRay(position);

            Physics.Raycast(origin, out RaycastHit hit);
            var ground = hit.collider.GetComponentInParent<Ground>();

            if (ground == null)
                return;
            if (!ground.Raised)
                return;
            
            List<Vector2> shifteds = new List<Vector2>();
            ground.SwapWaveAsync(shifteds);
            _ballSystem.SetTarget(ground.transform.position);
        }
    }
}