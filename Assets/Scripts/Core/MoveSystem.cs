using Rodser.Model;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rodser.Core
{
    internal class MoveSystem
    {
        private readonly InputSystem _input;
        private readonly HexGrid _hexGrid;

        public MoveSystem(InputSystem input, HexGrid hexGrid)
        {
            input.Click.performed += Move;
            _input = input;
            _hexGrid = hexGrid;
        }

        private void Move(InputAction.CallbackContext callback)
        {
            var position = _input.Position.ReadValue<Vector2>();
            var origin = Camera.main.ScreenPointToRay(position);

            Physics.Raycast(origin, out RaycastHit hit);
            var ground = hit.collider.GetComponentInParent<Ground>();

            if (ground == null)
                return;

            if (ground.Raised)
                _hexGrid.SwapPosition();
        }
    }
}