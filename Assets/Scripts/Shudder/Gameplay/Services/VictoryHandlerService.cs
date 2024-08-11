using BaCon;
using Shudder.Configs;
using Shudder.Events;
using Shudder.Services;
using Shudder.UI;
using UnityEngine;

namespace Shudder.Gameplay.Services
{
    public class VictoryHandlerService
    {
        private readonly DIContainer _container;
        private readonly GameConfig _gameConfig;
        private Transform _portalPoint;

        public VictoryHandlerService(DIContainer container, GameConfig gameConfig)
        {
            _container = container;
            _gameConfig = gameConfig;
            
            _container.Resolve<IReadOnlyEventBus>().PlayNextLevel.AddListener(OnPlayNextLevel);
        }

        public void OpenVictoryWindow(int coin, Transform portalPoint)
        {
            _portalPoint = portalPoint;
            _container.Resolve<InputService>().Disable();
            var prefab = _gameConfig.UIVictoryWindowView;
            var window = Object.Instantiate(prefab);
            window.Bind(_container.Resolve<ITriggerOnlyEventBus>(), _container.Resolve<InputService>());
            _container.Resolve<UIRootView>().AttachUI(window.gameObject);

            window.SetCoin(coin);
            window.ShowWindow();
        }

        private async void OnPlayNextLevel()
        {
            await _container
                .Resolve<CameraService>()
                .MoveCameraAsync(_portalPoint.position, 2f);
            
            _container.Resolve<UIRootView>().ShowLoadingScreen();
            
            await _container.Resolve<LevelLoadingService>().DestroyLevelAsync();
            await _container.Resolve<LevelLoadingService>().LoadAsync();
            _container.Resolve<UIRootView>().HideLoadingScreen();
        }
    }
}