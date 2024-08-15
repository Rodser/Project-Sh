using BaCon;
using Shudder.Events;
using Shudder.Gameplay.Services;
using Shudder.Models;
using Shudder.UI;
using UnityEngine;

namespace Shudder.Services
{
    public class SettingService
    {
        private readonly InputService _inputService;
        private readonly SfxService _sfxService;
        private readonly UIRootView _uiRootView;
        private readonly ITriggerOnlyEventBus _triggerOnlyEvent;
        
        private UISettingView _uiSettingViewPrefab;
        private LevelLoadingService _levelLoadingService;
        private CameraSurveillanceService _cameraSurveillanceService;
        private SceneActiveChecked _sceneActiveChecked;

        public SettingService(DIContainer container)
        {
            _inputService = container.Resolve<InputService>();
            _uiRootView = container.Resolve<UIRootView>();
            _sfxService = container.Resolve<SfxService>();
            _triggerOnlyEvent = container.Resolve<ITriggerOnlyEventBus>();
            var readOnlyEventBus = container.Resolve<IReadOnlyEventBus>();
            readOnlyEventBus.OpenSettings.AddListener(CreateSetting);
            readOnlyEventBus.RefreshLevel.AddListener(RefreshLevel);
            readOnlyEventBus.DieHero.AddListener(DieHero);
            readOnlyEventBus.LevelToMenu.AddListener(GoMenu);
        }

        public void Init(UISettingView uiSettingView, SceneActiveChecked sceneActiveChecked, LevelLoadingService levelLoadingService = null,
            CameraSurveillanceService cameraSurveillanceService = null)
        {
            _uiSettingViewPrefab = uiSettingView;
            _sceneActiveChecked = sceneActiveChecked;
            _levelLoadingService = levelLoadingService;
            _cameraSurveillanceService = cameraSurveillanceService;
        }

        private void DieHero()
        {
            if(_sceneActiveChecked.IsRun)
                RefreshLevel();
        }

        private void CreateSetting()
        {
            _inputService.Disable();

            var ui = Object.Instantiate(_uiSettingViewPrefab);
            ui.Bind(
                _triggerOnlyEvent,
                _inputService,
                _sfxService,
                _uiRootView.BundleVersion
                );
            _uiRootView.AttachUI(ui.gameObject);
            ui.ShowWindow();
        }

        private void GoMenu()
        {
            _sceneActiveChecked.IsRun = false;
            _cameraSurveillanceService.UnFollow();
            _inputService.Disable();
            _triggerOnlyEvent.TriggerGoMenu();
        }

        private async void RefreshLevel()
        {
            _sceneActiveChecked.IsRun = false;
            _cameraSurveillanceService?.UnFollow();
            _inputService.Disable();
            
            _uiRootView.ShowLoadingScreen();
            await _levelLoadingService.DestroyLevelAsync();
            await _levelLoadingService.LoadAsync();
            _uiRootView.HideLoadingScreen();
        }
    }
}