using BaCon;
using Cysharp.Threading.Tasks;
using Shudder.Configs;
using Shudder.Data;
using Shudder.Events;
using Shudder.Gameplay.Models.Interfaces;
using Shudder.Gameplay.Services;
using Shudder.Models;
using Shudder.Services;
using Shudder.UI;
using UnityEngine;
using Grid = Shudder.Models.Grid;

namespace Shudder.Gameplay.Root
{
    public class Game
    {
        private readonly StorageService _storage;
        private readonly GameConfig _gameConfig;
        private readonly CameraService _cameraService;
        private readonly CameraSurveillanceService _cameraSurveillanceService;
        private readonly InputService _inputService;
        private readonly CameraFollow _cameraFollow;
        private readonly RewardService _rewardService;
        private readonly VictoryHandlerService _victoryHandlerService;
        private readonly ShopService _shopService;

        public Game(DIContainer container, GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
            _storage = container.Resolve<StorageService>();
            _cameraService = container.Resolve<CameraService>();
            _inputService = container.Resolve<InputService>();
            _cameraSurveillanceService= container.Resolve<CameraSurveillanceService>();
            _cameraFollow = container.Resolve<CameraService>().CameraFollow;
            _rewardService = container.Resolve<RewardService>();
            _victoryHandlerService = container.Resolve<VictoryHandlerService>();
            _shopService = container.Resolve<ShopService>();
            
            var readOnlyEvent = container.Resolve<IReadOnlyEventBus>();
                readOnlyEvent.UpdateUI.AddListener(UpdateHud);
                readOnlyEvent.OpenShop.AddListener(OpenShop);
        }

        public SceneActiveChecked SceneActiveChecked { get; set; } = new();

        public IHero Hero { get; set; }

        public HudView HUD { get; set; }

        public Grid CurrentGrid { get; private set; }

        public async void Run()
        {
            var rotation = _gameConfig.CameraRotation;
            await _cameraService.MoveCameraAsync(Hero.Presenter.View.transform.position, rotation, 2.5f);
            
            _cameraSurveillanceService.Follow(_cameraFollow.Presenter.View, Hero);
            _inputService.Enable();
            
            Hero.Presenter.View.CanUsePortal();
            _rewardService.SetVictoryHandler(_victoryHandlerService);
            SceneActiveChecked.IsRun = true;
        }

        public void SetCurrentGrid(Grid currentGrid)
        {
            CurrentGrid = currentGrid;
        }

        public async UniTask DestroyGrid()
        {
            if(CurrentGrid is null)
                return;
            
            for (var x = 0; x < CurrentGrid.Grounds.GetLength(0); x++)
            {
                for (var y = 0; y < CurrentGrid.Grounds.GetLength(1); y++)
                {
                    var ground = CurrentGrid.Grounds[x, y];

                    if(ground.GroundType == GroundType.Pit)
                        continue;
                    if(ground.Presenter.View is not null)
                        Object.Destroy(ground.Presenter.View.gameObject);
                    ground.Presenter.View = null;
                    await UniTask.Yield();
                }
            }

            CurrentGrid.Grounds = null;
            Object.Destroy(CurrentGrid.Presenter.View.gameObject);
            CurrentGrid = null;
        }

        private void OpenShop()
        {
            _shopService.CreateShopWindow();
        }

        public void UpdateHud()
        {
            HUD.SetLevel(_storage.Progress.Level);
            HUD.SetCoin(_storage.Progress.Coin);
            HUD.SetDiamond(_storage.Progress.Diamond);
            HUD.SetJumpCount(_storage.Progress.JumpCount);
            HUD.SetWaveCount(_storage.Progress.MegaWave);
        }
    }
}