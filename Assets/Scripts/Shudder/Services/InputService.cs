using System;
using UnityEngine.InputSystem;

namespace Shudder.Gameplay.Services
{
    public class InputService : IDisposable
    {
        private readonly InputControl _inputAControl;
        private Action<InputAction.CallbackContext> _action;

        public InputService()
        {
            _inputAControl = new InputControl();
            Enable();
            
            Click = _inputAControl.FindAction("Click");
            Position = _inputAControl.FindAction("Position");
        }

        public InputAction Position { get; set; }
        public InputAction Click { get; set; }

        
        public void Enable()
        {
            _inputAControl.Enable();
        }

        public void Disable()
        {
            _inputAControl.Disable();
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

        public void Dispose()
        {
            _inputAControl.Actionmap.Disable();
        }
    }
}