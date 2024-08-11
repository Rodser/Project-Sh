using System;
using BaCon;
using Shudder.Constants;
using Shudder.Events;
using Shudder.MainMenu.Configs;
using Shudder.UI;
using UnityEngine;
using YG;
using Object = UnityEngine.Object;

namespace Shudder.Services
{
    public class RewardService: IDisposable
    {
        private MenuConfig _config;
        private readonly ITriggerOnlyEventBus _triggerOnlyEvent;
        private readonly InputService _inputService;
        private readonly UIRootView _uiRootView;
        private readonly CoinService _coinService;

        public RewardService(DIContainer container)
        {
            _triggerOnlyEvent = container.Resolve<ITriggerOnlyEventBus>();
            _inputService = container.Resolve<InputService>();
            _uiRootView = container.Resolve<UIRootView>();
            _coinService = container.Resolve<CoinService>();
            YandexGame.RewardVideoEvent += OnRewardVideoEvent;
        }

        public void Init(MenuConfig config)
        {
            _config = config;
        }

        private void OnRewardVideoEvent(int index)
        {
            Debug.Log($"OnRewardVideoEvent id : {index}");
            if(GameConstant.RewardIndex != index)
                return;
            var ui = CreateRewardWindow();
            var coin = _coinService.GetRewardedBonus();
            ui.SetCoin(coin);
            ui.ShowWindow();
        }

        private UIRewardWindowView CreateRewardWindow()
        {
            _inputService.Disable();
            var prefab = _config.UIRewardWindowView;
            var window = Object.Instantiate(prefab);
            window.Bind(_triggerOnlyEvent, _inputService);
            _uiRootView.AttachUI(window.gameObject);
            return window;
        }

        public void Dispose()
        {
            YandexGame.RewardVideoEvent -= OnRewardVideoEvent;
        }
    }
}