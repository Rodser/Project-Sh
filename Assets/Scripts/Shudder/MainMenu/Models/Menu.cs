using BaCon;
using Shudder.Data;
using Shudder.Events;
using Shudder.Gameplay.Models;
using Shudder.Gameplay.Services;
using Shudder.MainMenu.Configs;
using Shudder.Services;
using Shudder.UI;
using Grid = Shudder.Models.Grid;

namespace Shudder.MainMenu.Models
{
    public class Menu
    {
        private readonly DIContainer _container;
        private readonly Grid _menuGrid;
        private readonly MenuConfig _menuConfig;
        private readonly UIMenuView _menuView;
        private readonly Hero _hero;
        private readonly ITriggerOnlyEventBus _triggerEventBus;
        private readonly StorageService _storage;

        public Menu(DIContainer container, Grid menuGrid, MenuConfig menuConfig, UIMenuView menuView, Hero hero)
        {
            _container = container;
            _menuGrid = menuGrid;
            _menuConfig = menuConfig;
            _menuView = menuView;
            _hero = hero;

            _storage = container.Resolve<StorageService>();
            _storage.LoadProgress();
            _triggerEventBus = _container.Resolve<ITriggerOnlyEventBus>();
            var readOnlyEvent = _container.Resolve<IReadOnlyEventBus>();
            readOnlyEvent.PlayGame.AddListener(OnPlayGame);
            readOnlyEvent.UpdateUI.AddListener(OnUpdateUI);
        }
        
        public void UpdateProgress()
        {
            _menuView.SetProgress(_storage.Progress.GetLevelProgress());
        }
        
        public void OnUpdateUI()
        {
            var progress = _storage.Progress;
            
            _menuView.SetCoin(progress.Coin);
            _menuView.SetDiamond(progress.Diamond);
            _menuView.SetLevel(progress.Level);
        }

        private async void OnPlayGame()
        {
            var cameraService = _container.Resolve<CameraService>();
            await _container
                .Resolve<JumpService>()
                .Jump(
                    _menuConfig.HeroConfig.JumpConfig,
                    _hero.Presenter.View.transform,
                    _menuGrid.Portal.AnchorPoint
                );
            
            await cameraService.MoveCameraAsync(_menuGrid.Portal.AnchorPoint.position, 2f);
            
            _triggerEventBus.TriggerStartGameplayScene();
        }
    }
}