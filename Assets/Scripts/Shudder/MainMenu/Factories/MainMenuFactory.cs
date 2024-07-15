using DI;
using Logic;
using Shudder.Events;
using Shudder.Factories;
using Shudder.MainMenu.Configs;
using Shudder.Services;
using Shudder.UI;
using Shudder.Vews;
using UnityEngine;
using Grid = Shudder.Models.Grid;

namespace Shudder.MainMenu.Factories
{
    public class MainMenuFactory
    {
        private readonly DIContainer _container;
        private readonly MenuConfig _menuConfig;
        
        private Grid _menuGrid;
        private CameraFollowView _cameraView;
        private readonly ITriggerOnlyEventBus _triggerEventBus;

        public MainMenuFactory(DIContainer container, MenuConfig menuConfig)
        {
            _container = container;
            _menuConfig = menuConfig;
            
            _triggerEventBus = _container.Resolve<ITriggerOnlyEventBus>();
        }

        public async void Create()
        {
            _cameraView = _container.Resolve<CameraService>().View;
            _menuGrid = await _container
                .Resolve<GridFactory>("MenuGrid")
                .Create(-1, true);
            
            Object.Instantiate(_menuConfig.Title, _menuGrid.Hole.AnchorPoint);
            _container.Resolve<LightFactory>().Create(_menuConfig.Light, _cameraView.transform, _menuGrid.Presenter.View.transform);

            var menuUI = CreateUIMainMenu();
            menuUI.Bind(_triggerEventBus);
            var menu = new Models.MainMenu(_container, _menuGrid);
        }

        private UIMenuView CreateUIMainMenu()
        {
            var prefab = _menuConfig.UIMenuView;
            var menuUI = Object.Instantiate(prefab);
            _container.Resolve<UIRootView>().AttachSceneUI(menuUI.gameObject);
            return menuUI;
        }

    }
}