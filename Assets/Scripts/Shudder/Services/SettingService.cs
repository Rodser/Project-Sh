using BaCon;
using Shudder.Events;
using Shudder.Gameplay.Services;
using Shudder.UI;
using UnityEngine;

namespace Shudder.Services
{
    public class SettingService
    {
        private readonly DIContainer _container;

        private UISettingView _uiSettingViewPrefab;
        private LevelLoadingService _levelLoadingService;

        public SettingService(DIContainer container)
        {
            _container = container;
            var readOnlyEventBus = _container.Resolve<IReadOnlyEventBus>();
            
            readOnlyEventBus.OpenSettings.AddListener(CreateSetting);
            readOnlyEventBus.RefreshLevel.AddListener(RefreshLevel);
        }

        public void Init(UISettingView uiSettingView, LevelLoadingService levelLoadingService = null)
        {
            _uiSettingViewPrefab = uiSettingView;
            _levelLoadingService = levelLoadingService;
        }

        private void CreateSetting()
        {
            var ui = Object.Instantiate(_uiSettingViewPrefab);
            ui.Bind(_container.Resolve<ITriggerOnlyEventBus>());
            _container.Resolve<UIRootView>().AttachUI(ui.gameObject);
            ui.ShowWindow();
        }

        private async void RefreshLevel()
        {
            await _levelLoadingService.LoadAsync();
            _container.Resolve<UIRootView>().HideLoadingScreen();
        }
    }
}