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

        private Grid _currentGrid;

        public Game(DIContainer container, GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
            _storage = container.Resolve<StorageService>();
            _cameraService = container.Resolve<CameraService>();
            _inputService = container.Resolve<InputService>();
            _cameraSurveillanceService= container.Resolve<CameraSurveillanceService>();
            _cameraFollow = container.Resolve<CameraService>().CameraFollow;
            
            container.Resolve<IReadOnlyEventBus>().UpdateUI.AddListener(UpdateHud);
        }

        public IHero Hero { get; set; }
        public HudView HUD { get; set; }

        public async void Run()
        {
            var rotation = _gameConfig.CameraRotation;
            await _cameraService.MoveCameraAsync(Hero.Presenter.View.transform.position, rotation, 2.5f);
            
            _cameraSurveillanceService.Follow(_cameraFollow.Presenter.View, Hero);
            _inputService.Enable();
            
            Hero.Presenter.View.CanUsePortal();
        }

        public void SetCurrentGrid(Grid currentGrid)
        {
            _currentGrid = currentGrid;
        }

        public async UniTask DestroyGrid()
        {
            if(_currentGrid is null)
                return;
            
            for (var x = 0; x < _currentGrid.Grounds.GetLength(0); x++)
            {
                for (var y = 0; y < _currentGrid.Grounds.GetLength(1); y++)
                {
                    var ground = _currentGrid.Grounds[x, y];

                    if(ground.GroundType == GroundType.Pit)
                        continue;
                    Object.Destroy(ground.Presenter.View.gameObject);
                    ground.Presenter.View = null;
                    await UniTask.Yield();
                }
            }

            _currentGrid.Grounds = null;
            Object.Destroy(_currentGrid.Presenter.View.gameObject);
            _currentGrid = null;
        }

        public void UpdateHud()
        {
            HUD.SetLevel(_storage.Progress.Level);
            HUD.SetCoin(_storage.Progress.Coin);
            HUD.SetDiamond(_storage.Progress.Diamond);
        }
    }
}