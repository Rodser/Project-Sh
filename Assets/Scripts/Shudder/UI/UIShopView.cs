using Shudder.Events;
using Shudder.Services;
using TMPro;
using UnityEngine;

namespace Shudder.UI
{
    public class UIShopView : PopUpView
    {
        [SerializeField] private TextMeshProUGUI _coinTMP;
        [SerializeField] private TextMeshProUGUI _diamondTMP;
        [SerializeField] private TextMeshProUGUI _jumpCountTMP;
        [SerializeField] private TextMeshProUGUI _waveCountTMP;
        
        private ShopService _shopService;

        public void Bind(ITriggerOnlyEventBus eventBus, InputService inputService, ShopService shopService)
        {
            base.Bind(eventBus, inputService);
            _shopService = shopService;
        }
        
        public new void CloseWindow()
        {
            _shopService.UnSubscribe();
            base.CloseWindow();
        }
        
        public void BuySuperJump()
        {
            _shopService.BuySuperJump();
        }
        
        public void BuyMegaWave()
        {
            _shopService.BuyMegaWave();
        }
        
        public void SetJumpCount(int value)
        {
            _jumpCountTMP.text = $"x{value}";
        }
        
        public void SetWaveCount(int value)
        {
            _waveCountTMP.text = $"x{value}";
        }
        
        public void SetCoin(int value)
        {
            _coinTMP.text = value.ToString();
        }
        
        public void SetDiamond(int value)
        {
            _diamondTMP.text = value.ToString();
        }
    }
}