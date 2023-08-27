using UnityEngine.InputSystem;

namespace System
{
    public class InputSystem
    {
        public InputAction Click;
        public InputAction Position;
        private Action<InputAction.CallbackContext> _action;

        public void Initialize()
        {
            InputControl inputActions = new InputControl();
            inputActions.Enable();

            Click = inputActions.FindAction("Click");
            Position = inputActions.FindAction("Position");
        }

        public void AddListener(Action<InputAction.CallbackContext> action)
        {
            Click.performed += action;
            _action = action;
        }

        public void Clear()
        {
            Click.performed -= _action;
        }
    }
}