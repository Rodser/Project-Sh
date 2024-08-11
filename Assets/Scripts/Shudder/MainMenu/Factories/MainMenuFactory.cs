using BaCon;
using Cysharp.Threading.Tasks;
using Shudder.Events;
using Shudder.Factories;
using Shudder.Gameplay.Factories;
using Shudder.MainMenu.Configs;
using Shudder.MainMenu.Models;
using Shudder.Services;
using Shudder.UI;
using UnityEngine;
using Grid = Shudder.Models.Grid;

namespace Shudder.MainMenu.Factories
{
    public class MainMenuFactory
    {
        private readonly DIContainer _container;
        private readonly MenuConfig _menuConfig;
        
        private readonly ITriggerOnlyEventBus _triggerEventBus;
        private readonly CameraService _cameraService;

        public MainMenuFactory(DIContainer container, MenuConfig menuConfig)
        {
            _container = container;
            _menuConfig = menuConfig;
            
            _triggerEventBus = _container.Resolve<ITriggerOnlyEventBus>();
            _cameraService = _container.Resolve<CameraService>();
        }

        public async void Create()
        {
            _cameraService.Reset();
            var menuGrid = await CreateGrid();
            menuGrid.OffPortalCollider();
            
            _container.Resolve<LightFactory>().Create(_menuConfig.Lights, menuGrid, 0.2f);
            _container.Resolve<ItemFactory>().Create(_menuConfig.Items, menuGrid, 0.7f);
            _container.Resolve<SettingService>().Init(_menuConfig.UISettingView);
            
            CreateMusic(menuGrid);
            await MoveCamera();
            var hero = _container.Resolve<HeroFactory>().Create(menuGrid.Grounds);

            var menuUI = CreateUIMainMenu();
            var menu = new Menu(_container, menuGrid, _menuConfig, menuUI, hero);
            menu.OnUpdateUI();
            menu.UpdateProgress();
            menuUI.Bind(_triggerEventBus);
        }

        private async UniTask<Grid> CreateGrid()
        {
            var menuGrid = await _container
                .Resolve<GridFactory>("MenuGrid")
                .Create(-1, true);
            return menuGrid;
        }

        private void CreateMusic(Grid grid)
        {
            var service = _container.Resolve<SfxService>();
                
            service.CreateMusicMenu(_menuConfig.SfxConfig, grid.Presenter.View.transform);
            service.StartMusicMenu();
        }
        
        private async UniTask MoveCamera()
        {
            var position = _menuConfig.CameraPosition; 
            var rotation = _menuConfig.CameraRotation; 
            await _cameraService.MoveCameraAsync(position, rotation);
        }

        private UIMenuView CreateUIMainMenu()
        {
            var prefab = _menuConfig.UIMenuView;
            var menuUI = Object.Instantiate(prefab);
            _container.Resolve<UIRootView>().ChangeSceneUI(menuUI.gameObject);
            return menuUI;
        }
    }
}