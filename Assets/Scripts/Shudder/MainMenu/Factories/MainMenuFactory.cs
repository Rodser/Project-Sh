using Cysharp.Threading.Tasks;
using DI;
using Shudder.Events;
using Shudder.Factories;
using Shudder.Gameplay.Factories;
using Shudder.MainMenu.Configs;
using Shudder.Services;
using Shudder.UI;
using UnityEngine;

namespace Shudder.MainMenu.Factories
{
    public class MainMenuFactory
    {
        private readonly DIContainer _container;
        private readonly MenuConfig _menuConfig;
        
        private readonly ITriggerOnlyEventBus _triggerEventBus;

        public MainMenuFactory(DIContainer container, MenuConfig menuConfig)
        {
            _container = container;
            _menuConfig = menuConfig;
            
            _triggerEventBus = _container.Resolve<ITriggerOnlyEventBus>();
        }

        public async void Create()
        {
            var menuGrid = await _container
                .Resolve<GridFactory>("MenuGrid")
                .Create(-1, true);
            
            var hero = _container.Resolve<HeroFactory>().Create(menuGrid.Grounds);
            _container.Resolve<LightFactory>().Create(_menuConfig.Lights, menuGrid);
            _container.Resolve<ItemFactory>().Create(_menuConfig.Items, menuGrid);
            await MoveCamera();

            var menuUI = CreateUIMainMenu();
            menuUI.Bind(_triggerEventBus);
            var menu = new Models.MainMenu(_container, menuGrid, _menuConfig, hero);
        }

        private async UniTask MoveCamera()
        {
            var position = _menuConfig.CameraPosition; 
            var rotation = _menuConfig.CameraRotation; 
            await _container.Resolve<CameraService>().MoveCameraAsync(position, rotation);
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