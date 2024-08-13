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
        private readonly UIRootView _uiRootView;
        private readonly SfxService _sfxService;
        private readonly InputService _inputService;
        private readonly ITriggerOnlyEventBus _triggerOnlyEvent;
        private readonly LevelLoadingService _levelLoadingService;
        private readonly CameraService _cameraService;
        
        private Transform _portalPoint;

        public VictoryHandlerService(DIContainer container, GameConfig gameConfig)
        {
            _container = container;
            _gameConfig = gameConfig;
            _inputService = container.Resolve<InputService>();
            _triggerOnlyEvent = container.Resolve<ITriggerOnlyEventBus>();
            _sfxService = container.Resolve<SfxService>();
            _uiRootView = container.Resolve<UIRootView>();
            _cameraService = container.Resolve<CameraService>();
            
            container.Resolve<IReadOnlyEventBus>().PlayNextLevel.AddListener(OnPlayNextLevel);
        }

        public void OpenVictoryWindow(int coin, Transform portalPoint)
        {
            _portalPoint = portalPoint;
            _inputService.Disable();
            _sfxService.StopMusic();
            var prefab = _gameConfig.UIVictoryWindowView;
            var window = Object.Instantiate(prefab);
            window.Bind(_triggerOnlyEvent, _inputService);
            _uiRootView.AttachUI(window.gameObject);

            window.SetCoin(coin);
            window.ShowWindow();
        }

        private async void OnPlayNextLevel()
        {
            await _cameraService.MoveCameraAsync(_portalPoint.position, 2f);
            
            _uiRootView.ShowLoadingScreen();
            var levelLoadingService = _container.Resolve<LevelLoadingService>();
            await levelLoadingService.DestroyLevelAsync();
            await levelLoadingService.LoadAsync();
            _uiRootView.HideLoadingScreen();
        }
    }
}