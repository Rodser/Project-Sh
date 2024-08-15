using Cysharp.Threading.Tasks;
using DG.Tweening;
using Shudder.Events;
using Shudder.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Shudder.UI
{
    public class UISettingView : PopUpView
    {
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _soundSlider;
        [SerializeField] private TextMeshProUGUI _versionTMPro;
       
        public void Bind(ITriggerOnlyEventBus eventBus, InputService inputService, SfxService sfxService,
            string version)
        {
            base.Bind(eventBus, inputService);
            _musicSlider.value = sfxService.MusicMute;
            _soundSlider.value = sfxService.SoundMute;
            SetVersion(version);
        }
        
        public void ChangeMusicSlider()
        {        
            Debug.Log($"Music {_musicSlider.value}");
            _triggerOnlyEvent.TriggerMusicMute(_musicSlider.value);
        }
        
        public void ChangeSoundSlider()
        {        
            Debug.Log($"Sound {_soundSlider.value}");
            _triggerOnlyEvent.TriggerSoundMute(_soundSlider.value);
        }

        public void ResetProgress()
        {
            YandexGame.ResetSaveProgress();
            _triggerOnlyEvent.TriggerGoMenu();
        }
        
        public async void GoToMenu()
        {        
            await UniTask.Delay(400);
            _triggerOnlyEvent.TriggerLevelToMenu();
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

        private void SetVersion(string bundleVersion)
        {
            _versionTMPro.text = $"v. {bundleVersion}";
        }
    }
}