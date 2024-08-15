using System;
using BaCon;
using Shudder.Constants;
using Shudder.Events;
using Shudder.Gameplay.Services;
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
        private VictoryHandlerService _victoryHandlerService;

        public RewardService(DIContainer container)
        {
            _triggerOnlyEvent = container.Resolve<ITriggerOnlyEventBus>();
            _inputService = container.Resolve<InputService>();
            _uiRootView = container.Resolve<UIRootView>();
            _coinService = container.Resolve<CoinService>();
            YandexGame.RewardVideoEvent += OnRewardVideoEvent;
            YandexGame.ErrorVideoEvent += OnErrorVideo;
        }

        public void Init(MenuConfig config)
        {
            _config = config;
        }

        private void OnRewardVideoEvent(int index)
        {
            Debug.Log($"OnRewardVideoEvent id : {index}");
            switch (index)
            {
                case GameConstant.RewardIndex:
                {
                    var ui = CreateRewardWindow();
                    var coin = _coinService.GetRewardedBonus();
                    ui.SetCoin(coin);
                    ui.ShowWindow();
                    break;
                }
                case GameConstant.RewardIndexNextLevel:
                {
                    var ui = CreateRewardWindow(true);
                    var coin = _coinService.GetRewardedNextLevelBonus();
                    ui.SetCoin(coin);
                    ui.ShowWindow();
                    break;
                }
            }
        }

        private UIRewardWindowView CreateRewardWindow(bool nextLevel = false)
        {
            _inputService.Disable();
            var prefab = _config.UIRewardWindowView;
            var window = Object.Instantiate(prefab);
            window.Bind(_triggerOnlyEvent, _inputService, nextLevel);
            _uiRootView.AttachUI(window.gameObject);
            return window;
        }

        private void OnErrorVideo()
        {
            var coin = _coinService.GetEarnedCoin();
            _victoryHandlerService.OpenWindow(coin);
        }

        public void Dispose()
        {
            YandexGame.RewardVideoEvent -= OnRewardVideoEvent;
            YandexGame.ErrorVideoEvent -= OnErrorVideo;

        }

        public void SetVictoryHandler(VictoryHandlerService victoryHandlerService) =>
            _victoryHandlerService = victoryHandlerService;
    }
}