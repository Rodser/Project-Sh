using BaCon;
using Shudder.Data;
using Shudder.Events;
using Shudder.Gameplay.Models;
using Shudder.Gameplay.Services;
using Shudder.MainMenu.Configs;
using Shudder.Models;
using Shudder.Services;
using Shudder.UI;
using UnityEngine;
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
        private readonly LeaderBoardsService _leaderboardsService;

        public Menu(DIContainer container, Grid menuGrid, MenuConfig menuConfig, UIMenuView menuView, Hero hero)
        {
            _container = container;
            _menuGrid = menuGrid;
            _menuConfig = menuConfig;
            _menuView = menuView;
            _hero = hero;

            _storage = container.Resolve<StorageService>();
            _leaderboardsService = container.Resolve<LeaderBoardsService>();
            _storage.LoadProgress();
            _triggerEventBus = _container.Resolve<ITriggerOnlyEventBus>();
            SceneActiveChecked.IsRun = false;
            
            var readOnlyEvent = _container.Resolve<IReadOnlyEventBus>();
            readOnlyEvent.PlayGame.AddListener(OnPlayGame);
            readOnlyEvent.UpdateUI.AddListener(OnUpdateUI);
            readOnlyEvent.OpenLeaderboards.AddListener(OnOpenLeaderboards);
        }

        public SceneActiveChecked SceneActiveChecked { get; set; } = new();

        private void OnOpenLeaderboards()
        {
           _leaderboardsService.CreateRewardWindow();
        }

        public void UpdateProgressBar()
        {
            _menuView.SetProgressBar(_storage.Progress.GetLevelProgress());
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
            SceneActiveChecked.IsRun = false;
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