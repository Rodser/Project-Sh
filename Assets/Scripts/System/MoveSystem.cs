using Rodser.Model;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Rodser.System
{
    internal class MoveSystem
    {
        private readonly InputSystem _input;

        public MoveSystem(InputSystem input)
        {
            input.Click.performed += Move;
            _input = input;
        }

        private void Move(InputAction.CallbackContext callback)
        {
            var position = _input.Position.ReadValue<Vector2>();
            var origin = Camera.main.ScreenPointToRay(position);

            Physics.Raycast(origin, out RaycastHit hit);
            var ground = hit.collider.GetComponentInParent<Ground>();

            if (ground == null)
                return;

            Debug.Log(ground.Raised);
            if (ground.Raised)
            {
                List<Vector2> shifteds = new List<Vector2>();
                ground.SwapWaveAsync(shifteds);
            }
        }
    }
}