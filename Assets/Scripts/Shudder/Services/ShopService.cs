using BaCon;
using Shudder.Configs;
using Shudder.Constants;
using Shudder.Data;
using Shudder.Events;
using Shudder.MainMenu.Configs;
using Shudder.UI;
using UnityEngine;

namespace Shudder.Services
{
    public class ShopService
    {
        private readonly InputService _inputService;
        private readonly ITriggerOnlyEventBus _triggerOnlyEvent;
        private readonly UIRootView _uiRootView;
        private readonly StorageService _storageService;
        private readonly IReadOnlyEventBus _readOnlyEvent;
        
        private MenuConfig _config;
        private UIShopView _shopView;
        private PriceInfo _price;

        public ShopService(DIContainer container)
        {
            _inputService = container.Resolve<InputService>();
            _triggerOnlyEvent = container.Resolve<ITriggerOnlyEventBus>();
            _uiRootView = container.Resolve<UIRootView>();
            _storageService = container.Resolve<StorageService>();
            _readOnlyEvent = container.Resolve<IReadOnlyEventBus>();
        }

        public void Init(MenuConfig config)
        {
            _config = config;
            _price = LoadPrice();
        }

        public void UnSubscribe()
        {
            _readOnlyEvent.UpdateUI.RemoveListener(UpdateUI);
        }

        public void CreateShopWindow()
        {
            Debug.Log("CreateRewardWindow");
            _inputService.Disable();
            _shopView = Object.Instantiate(_config.UIShopView);
            _shopView.Bind(_triggerOnlyEvent, _inputService, this);
            _uiRootView.AttachUI(_shopView.gameObject);
            
            _shopView.SetJumpPrice(_price.Jump);
            _shopView.SetWavePrice(_price.Wave);
            UpdateUI();
            _readOnlyEvent.UpdateUI.AddListener(UpdateUI);
        }

        public void BuySuperJump()
        {
            var superJumpPrice = _price.Jump;
            
            _storageService.UpJumpCount(superJumpPrice);
        }

        public void BuyMegaWave()
        {
            var wavePrice = _price.Wave;
            
            _storageService.UpMegaWave(wavePrice);
        }

        public static PriceInfo LoadPrice()
        {
            return Resources.Load<PriceInfo>(GameConstant.PricePath);
        }

        private void UpdateUI()
        {
            _shopView.SetCoin(_storageService.Progress.Coin);
            _shopView.SetDiamond(_storageService.Progress.Diamond);
            _shopView.SetJumpCount(_storageService.Progress.JumpCount);
            _shopView.SetWaveCount(_storageService.Progress.MegaWave);
        }
    }
}