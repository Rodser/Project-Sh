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
        private readonly ItemFactory _itemFactory;
        private readonly SettingService _settingService;
        private readonly HeroFactory _heroFactory;
        private readonly GridFactory _gridFactory;
        private readonly SfxService _sfxService;
        private readonly UIRootView _uiRootView;

        public MainMenuFactory(DIContainer container, MenuConfig menuConfig)
        {
            _container = container;
            _menuConfig = menuConfig;
            
            _triggerEventBus = _container.Resolve<ITriggerOnlyEventBus>();
            _cameraService = _container.Resolve<CameraService>();
            _itemFactory = _container.Resolve<ItemFactory>();
            _settingService = _container.Resolve<SettingService>();
            _heroFactory = _container.Resolve<HeroFactory>();
            _gridFactory = _container.Resolve<GridFactory>("MenuGrid");
            _sfxService = _container.Resolve<SfxService>();
            _uiRootView = _container.Resolve<UIRootView>();
        }

        public async void Create()
        {
            _cameraService.Reset();
            var menuGrid = await CreateGrid();
            menuGrid.OffPortalCollider();
            
            _itemFactory.Create(
                _menuConfig.Coin,
                menuGrid, 
                _menuConfig.MenuGridConfig.CountCoin,
                _menuConfig.MenuGridConfig.ChanceCoin);
            _itemFactory.Create(_menuConfig.Items, menuGrid, 0.7f);
            
            CreateMusic();
            await MoveCamera();
            var hero = _heroFactory.Create(menuGrid.Grounds);

            var menuUI = CreateUIMainMenu();
            var menu = new Menu(_container, menuGrid, _menuConfig, menuUI, hero);
            menu.UpdateUI();
            menu.UpdateProgressBar();
            menuUI.Bind(_triggerEventBus);
            _settingService.Init(_menuConfig.UISettingView, menu.SceneActiveChecked);
        }

        private async UniTask<Grid> CreateGrid() => 
            await _gridFactory.Create(-1, true);

        private void CreateMusic() => 
            _sfxService.CreateMusicMenu(_menuConfig.SfxConfig);

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
            _uiRootView.ChangeSceneUI(menuUI.gameObject);
            return menuUI;
        }
    }
}