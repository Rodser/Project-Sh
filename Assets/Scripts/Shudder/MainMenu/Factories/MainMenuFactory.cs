using Config;
using Core;
using DI;
using Logic;
using Model;
using Shudder.Gameplay.Services;
using Shudder.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Shudder.MainMenu.Factories
{
    public class MainMenuFactory
    {
        public event UnityAction OnStartLevel;

        private readonly DIContainer _container;
        private readonly MenuConfig _menu;
        private readonly InputService _input;
        
        private BodyGrid _body;
        private HexogenGrid _menuGrid;
        private Camera _camera;

        public MainMenuFactory(DIContainer container, MenuConfig menu)
        {
            _container = container;
            _menu = menu;
        
            _input = _container.Resolve<InputService>();

            OnStartLevel += StartLevel;
        }

        public async void Create()
        {
            _camera = _container.Resolve<CameraService>().Camera;
            _body = _container.Resolve<BodyFactory>().Create();

            _menuGrid = await _container.Resolve<GridFactory>("MenuGrid").Create(_body.transform, true);
            
            Object.Instantiate(_menu.Title, _menuGrid.Hole.transform);
            _container.Resolve<LightFactory>().Create(_menu.Light, _camera.transform, _body.transform);

            var menuUI = LoadInterface();
        }

        private UIMenuView LoadInterface()
        {
            var prefab = _menu.UIMenuView;
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