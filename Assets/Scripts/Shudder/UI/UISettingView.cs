using Cysharp.Threading.Tasks;
using DG.Tweening;
using Shudder.Events;
using Shudder.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Shudder.UI
{
    public class UISettingView : MonoBehaviour
    {
        private const float AnimDuration = 0.2f;
       
        [field: SerializeField] public Slider MusicSlider { get; private set; }    
        [field: SerializeField] public Slider SoundSlider { get; private set; }

        [SerializeField] private Transform _window;
        
        private ITriggerOnlyEventBus _triggerOnlyEventBus;
        private InputService _inputService;

        public void Bind(ITriggerOnlyEventBus eventBus, InputService inputService)
        {
            _inputService = inputService;
            _triggerOnlyEventBus = eventBus;
        }
        
        public void ChangeMusicSlider()
        {        
            Debug.Log($"Music {MusicSlider.value}");
        }
        
        public void ChangeSoundSlider()
        {        
            Debug.Log($"Sound {SoundSlider.value}");
        }
        
        public async void GoToMenu()
        {        
            await UniTask.Delay(400);
            _triggerOnlyEventBus.TriggerGoMenu();
            Debug.Log("Exit to menu");
            CloseWindow();
        }
        
        public async void RefreshLevel()
        {
            var tween = _window.DOScale(Vector3.zero, AnimDuration);
            await tween.AsyncWaitForCompletion();
            _triggerOnlyEventBus.TriggerRefreshLevel();
            Debug.Log("Refresh level");
            Destroy(gameObject);
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