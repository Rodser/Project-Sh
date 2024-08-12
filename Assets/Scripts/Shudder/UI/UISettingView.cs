using Cysharp.Threading.Tasks;
using DG.Tweening;
using Shudder.Events;
using Shudder.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Shudder.UI
{
    public class UISettingView : PopUpView
    {
        [field: SerializeField] public Slider MusicSlider { get; private set; }    
        [field: SerializeField] public Slider SoundSlider { get; private set; }
        
        public void Bind(ITriggerOnlyEventBus eventBus, InputService inputService, SfxService sfxService)
        {
            base.Bind(eventBus, inputService);
            MusicSlider.value = sfxService.MusicMute;
            SoundSlider.value = sfxService.SoundMute;
        }
        
        public void ChangeMusicSlider()
        {        
            Debug.Log($"Music {MusicSlider.value}");
            _triggerOnlyEvent.TriggerMusicMute(MusicSlider.value);
        }
        
        public void ChangeSoundSlider()
        {        
            Debug.Log($"Sound {SoundSlider.value}");
            _triggerOnlyEvent.TriggerSoundMute(SoundSlider.value);
        }
        
        public async void GoToMenu()
        {        
            await UniTask.Delay(400);
            _triggerOnlyEvent.TriggerGoMenu();
            Debug.Log("Exit to menu");
            CloseWindow();
        }
        
        public async void RefreshLevel()
        {
            var tween = _window.DOScale(Vector3.zero, AnimDuration);
            await tween.AsyncWaitForCompletion();
            _triggerOnlyEvent.TriggerRefreshLevel();
            Debug.Log("Refresh level");
            Destroy(gameObject);
        }
    }
}