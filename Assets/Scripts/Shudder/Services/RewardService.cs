using System;
using BaCon;
using Shudder.Constants;
using Shudder.Events;
using Shudder.MainMenu.Configs;
using Shudder.UI;
using UnityEngine;
using YG;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Shudder.Services
{
    public class RewardService: IDisposable
    {
        private readonly DIContainer _container;
        private MenuConfig _config;

        public RewardService(DIContainer container)
        {
            _container = container;
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
            var coin = Random.Range(1, 10) * 50;
            ui.SetCoin(coin);
            ui.ShowWindow();
        }

        private UIRewardWindowView CreateRewardWindow()
        {
            _container.Resolve<InputService>().Disable();
            var prefab = _config.UIRewardWindowView;
            var window = Object.Instantiate(prefab);
            window.Bind(_container.Resolve<ITriggerOnlyEventBus>(), _container.Resolve<InputService>());
            _container.Resolve<UIRootView>().AttachUI(window.gameObject);
            return window;
        }

        public void Dispose()
        {
            YandexGame.RewardVideoEvent -= OnRewardVideoEvent;
        }
    }
}