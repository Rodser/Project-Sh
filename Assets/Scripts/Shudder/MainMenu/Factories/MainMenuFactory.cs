using Config;
using Core;
using DI;
using Logic;
using Model;
using Shudder.Events;
using Shudder.Gameplay.Services;
using Shudder.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Shudder.MainMenu.Factories
{
    public class MainMenuFactory
    {
        private readonly DIContainer _container;
        private readonly MenuConfig _menuConfig;
        private readonly InputService _input;
        private readonly EventBus _eventBus;
        
        private BodyGrid _body;
        private HexogenGrid _menuGrid;
        private Camera _camera;

        public MainMenuFactory(DIContainer container, MenuConfig menuConfig)
        {
            _container = container;
            _menuConfig = menuConfig;
        
            _input = _container.Resolve<InputService>();
            _eventBus = _container.Resolve<EventBus>();
        }

        public async void Create()
        {
            _camera = _container.Resolve<CameraService>().Camera;
            _body = _container.Resolve<BodyFactory>().Create();

            _menuGrid = await _container.Resolve<GridFactory>("MenuGrid").Create(_body.transform, true);
            
            Object.Instantiate(_menuConfig.Title, _menuGrid.Hole.transform);
            _container.Resolve<LightFactory>().Create(_menuConfig.Light, _camera.transform, _body.transform);

            var menuUI = LoadInterface();
            menuUI.Bind(_eventBus);
        }

        private UIMenuView LoadInterface()
        {
            var prefab = _menuConfig.UIMenuView;
            var menuUI = Object.Instantiate(prefab);
            _container.Resolve<UIRootView>().AttachSceneUI(menuUI.gameObject);
            return menuUI;
        }

        private void StartLevel()
        {
            StartGameplayAsync(); // start gameplay
        }

        private async void StartGameplayAsync()
        {
            var cameraService = _container.Resolve<CameraService>();
            await cameraService.MoveCameraAsync(_menuGrid.Hole.transform.position);
            
            // start gameolay
        }
    }
}