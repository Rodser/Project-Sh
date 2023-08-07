using UnityEngine.InputSystem;

namespace Rodser.System
{
    public class InputSystem
    {
        public InputAction Click;
        public InputAction Position;

        public void Initialize()
        {
            InputControl inputActions = new InputControl();
            inputActions.Enable();

            Click = inputActions.FindAction("Click");
            Position = inputActions.FindAction("Position");
        }
    }
}