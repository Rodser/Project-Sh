using BaCon;
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
            _readOnlyEvent.UpdateUI.AddListener(UpdateUI);
            UpdateUI();
        }

        public void BuySuperJump()
        {
            var superJumpPrice = 9; // TODO: Create Price
            
            _storageService.UpJumpCount(superJumpPrice);
        }
        
        public void BuyMegaWave()
        {
            var wavePrice = 19; // TODO: Create Price
            
            _storageService.UpMegaWave(wavePrice);
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