using DG.Tweening;
using Shudder.Events;
using Shudder.Services;
using UnityEngine;

namespace Shudder.UI
{
    public abstract class PopUpView : MonoBehaviour
    {
        protected const float AnimDuration = 0.2f;

        [SerializeField] protected Transform _window;
        
        protected ITriggerOnlyEventBus _triggerOnlyEvent;
        protected InputService _inputService;

        public void Bind(ITriggerOnlyEventBus eventBus, InputService inputService)
        {
            _inputService = inputService;
            _triggerOnlyEvent = eventBus;
        }
        
        public void ShowWindow()
        {
            _window.localScale = Vector3.zero;
            _window.DOScale(Vector3.one, AnimDuration);
        }

        public async void CloseWindow()
        {
            var tween = _window.DOScale(Vector3.zero, AnimDuration);
            await tween.AsyncWaitForCompletion();
            _inputService.Enable();
            Destroy(gameObject);
        }
    }
}