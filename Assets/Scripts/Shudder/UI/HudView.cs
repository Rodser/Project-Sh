using BaCon;
using DG.Tweening;
using Shudder.Constants;
using Shudder.Events;
using Shudder.Gameplay.Services;
using Shudder.Services;
using TMPro;
using UnityEngine;
using YG;

namespace Shudder.UI
{
    public class HudView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _LevelTMP;
        [SerializeField] private TextMeshProUGUI _coinTMP;
        [SerializeField] private TextMeshProUGUI _diamondTMP;
        [SerializeField] private TextMeshProUGUI _jumpCountTMP;
        [SerializeField] private TextMeshProUGUI _waveCountTMP;

        private ITriggerOnlyEventBus _triggerOnlyEventBus;
        private SwapService _swapService;
        private SfxService _sfxService;
        private int _currentDiamond;
        private int _currentCoin;

        public void Bind(DIContainer container)
        {
            _triggerOnlyEventBus = container.Resolve<ITriggerOnlyEventBus>();
            _swapService = container.Resolve<SwapService>();
            _sfxService = container.Resolve<SfxService>();
        }
        
        public void Reward()
        {
            _sfxService.Click();
            YandexGame.RewVideoShow(GameConstant.RewardIndex);
        }
        
        public void OpenSettings()
        {
            _sfxService.Click();
            _triggerOnlyEventBus.TriggerOpenSettings();
        }
        
        public void OpenShop()
        {
            _sfxService.Click();
            _triggerOnlyEventBus.TriggerOpenShop();
        }
         
        public void RunMegaWave()
        {
            _sfxService.Click();
            _swapService.RunMegaWave();
        }
        
        public void ActivateSuperJump()
        {
            _sfxService.Click();
            _triggerOnlyEventBus.TriggerActivateSuperJump();
        }
        
        public void SetJumpCount(int value)
        {
            _jumpCountTMP.text = $"x{value}";
        }
        
        public void SetWaveCount(int value)
        {
            _waveCountTMP.text = $"x{value}";
        }
        
        public void SetLevel(int value)
        {
            value++;
            _LevelTMP.text = value.ToString();
        }
        
        public void SetCoin(int value)
        {
            _coinTMP?.transform.localScale.Set(1f,1f,1f);
            if(_currentCoin == value)
                return;
            _currentCoin = value;
            _coinTMP.text = value.ToString();
            _coinTMP?.transform.DOPunchScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f).SetLink(_coinTMP.gameObject);
        }
        
        public void SetDiamond(int value)
        {
            _diamondTMP?.transform.localScale.Set(1f,1f,1f);
            if(_currentDiamond == value)
                return;
            _currentDiamond = value;
            _diamondTMP.text = value.ToString();
            _diamondTMP?.transform.DOPunchScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f).SetLink(_diamondTMP.gameObject);
        }
    }
}