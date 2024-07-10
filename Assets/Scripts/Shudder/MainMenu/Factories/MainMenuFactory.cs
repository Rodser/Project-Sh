using DI;
using Logic;
using Shudder.Events;
using Shudder.Gameplay.Factories;
using Shudder.Gameplay.Models;
using Shudder.Gameplay.Services;
using Shudder.MainMenu.Configs;
using Shudder.UI;
using UnityEngine;

namespace Shudder.MainMenu.Factories
{
    public class MainMenuFactory
    {
        private readonly DIContainer _container;
        private readonly MenuConfig _menuConfig;
        
        private BodyGrid _body;
        private HexogenGrid _menuGrid;
        private Camera _camera;
        private readonly ITriggerOnlyEventBus _triggerEventBus;

        public MainMenuFactory(DIContainer container, MenuConfig menuConfig)
        {
            _container = container;
            _menuConfig = menuConfig;
            
            _triggerEventBus = _container.Resolve<ITriggerOnlyEventBus>();
        }

        public async void Create()
        {
            _camera = _container.Resolve<CameraService>().Camera;
            _body = _container.Resolve<BodyFactory>().Create();
            _menuGrid = await _container
                .Resolve<GridFactory>("MenuGrid")
                .Create(_body.transform, true);
            
            Object.Instantiate(_menuConfig.Title, _menuGrid.Hole.AnchorPoint);
            _container.Resolve<LightFactory>().Create(_menuConfig.Light, _camera.transform, _body.transform);

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