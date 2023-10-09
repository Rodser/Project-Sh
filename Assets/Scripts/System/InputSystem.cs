using UnityEngine.InputSystem;

namespace System
{
    public class InputSystem
    {
        public InputAction Click;
        public InputAction Position;
        private Action<InputAction.CallbackContext> _action;
        private InputControl _inputActions;

        public void Initialize()
        {
            _inputActions = new InputControl();
            Enable();
            
            Click = _inputActions.FindAction("Click");
            Position = _inputActions.FindAction("Position");
        }

        public void Enable()
        {
            _inputActions.Enable();
        }

        public void Disable()
        {
            _inputActions.Disable();
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