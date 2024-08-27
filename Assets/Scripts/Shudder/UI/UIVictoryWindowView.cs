using Cysharp.Threading.Tasks;
using Shudder.Constants;
using Shudder.Events;
using Shudder.Services;
using TMPro;
using UnityEngine;
using YG;

namespace Shudder.UI
{
    public class UIVictoryWindowView : PopUpView
    {
        [SerializeField] private TextMeshProUGUI _coinTM;
        
        private SfxService _sfxService;

        public void Bind(ITriggerOnlyEventBus triggerOnlyEvent, InputService inputService, SfxService sfxService)
        {
            base.Bind(triggerOnlyEvent, inputService);
            _sfxService = sfxService;
        }

        public void PlayNextLevel()
        {
            _sfxService.Click();
            Debug.Log("Next Level");
            _triggerOnlyEvent.TriggerPlayNextLevel();
            CloseWindow();
        }

        public async void RewardToNext()
        {        
            _sfxService.Click();
            await UniTask.Delay(400);
            Debug.Log("Reward To Next");
            CloseWindow();
            YandexGame.RewVideoShow(GameConstant.RewardIndexNextLevel);
        }

        public void SetCoin(int value)
        {
            _coinTM.text = $"+{value}";
        }
    }
}