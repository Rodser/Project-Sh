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
        [Space]
        [SerializeField] private TextMeshProUGUI _jumpCountTMP;
        [SerializeField] private TextMeshProUGUI _waveCountTMP;
        [Space]
        [SerializeField] private TextMeshProUGUI _jumpPriceTMP;
        [SerializeField] private TextMeshProUGUI _wavePriceTMP;
        [Space]
        [SerializeField] private TextMeshProUGUI _jumpPriceXTMP;
        [SerializeField] private TextMeshProUGUI _wavePriceXTMP;
        
        private ShopService _shopService;
        private SfxService _sfxService;

        public void Bind(ITriggerOnlyEventBus eventBus, InputService inputService, SfxService sfxService, ShopService shopService)
        {
            base.Bind(eventBus, inputService);
            _sfxService = sfxService;
            _shopService = shopService;
        }
        
        public new void CloseWindow()
        {
            _sfxService.Click();
            _shopService.UnSubscribe();
            base.CloseWindow();
        }
        
        public void BuySuperJump(int count = 1)
        {
            _sfxService.Click();
            _shopService.BuySuperJump(count);
        }

        public void BuyMegaWave(int count = 1)
        {
            _sfxService.Click();
            _shopService.BuyMegaWave(count);
        }

        public void SetJumpCount(int value) => 
            _jumpCountTMP.text = $"x{value}";

        public void SetWaveCount(int value) => 
            _waveCountTMP.text = $"x{value}";

        public void SetJumpPrice(int value) => 
            _jumpPriceTMP.text = $"{value}";

        public void SetWavePrice(int value) => 
            _wavePriceTMP.text = $"{value}";

        public void SetJumpPriceX(int value) => 
            _jumpPriceXTMP.text = $"{value}";

        public void SetWavePriceX(int value) => 
            _wavePriceXTMP.text = $"{value}";
        
        public void SetCoin(int value) => 
            _coinTMP.text = value.ToString();

        public void SetDiamond(int value) => 
            _diamondTMP.text = value.ToString();
    }
}