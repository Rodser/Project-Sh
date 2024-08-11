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
        private readonly DIContainer _container;
        private readonly GameConfig _gameConfig;

        public PlayerProgress Progress { get; private set; }
        public CameraFollow CameraFollow { get; set; }
        public Grid CurrentGrid { get; private set; }
        public IHero Hero { get; set; }
        public HudView HUD { get; set; }

        public Game(DIContainer container, GameConfig gameConfig)
        {
            _container = container;
            _gameConfig = gameConfig;
            CameraFollow = _container.Resolve<CameraService>().CameraFollow;
        }

        public async void Run()
        {
            var rotation = _gameConfig.CameraRotation;
            await _container
                .Resolve<CameraService>()
                .MoveCameraAsync(Hero.Presenter.View.transform.position, rotation, 2.5f);

            _container.Resolve<IReadOnlyEventBus>().UpdateCoin.AddListener(UpdateHud);
            _container.Resolve<CameraSurveillanceService>().Follow(CameraFollow.Presenter.View, Hero);
            _container.Resolve<InputService>().Enable();
            
            Hero.Presenter.View.CanUsePortal();
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
                    Object.Destroy(ground.Presenter.View.gameObject);
                    ground.Presenter.View = null;
                    await UniTask.Yield();
                }
            }

            CurrentGrid.Grounds = null;
            Object.Destroy(CurrentGrid.Presenter.View.gameObject);
            CurrentGrid = null;
        }

        public void UpLevel()
        {
            if (Progress.Level >= _gameConfig.LevelGridConfigs.Length - 1) 
                return;
            
            Progress.Coin += MakeMoney();
            Progress.Level++;
            _container.Resolve<StorageService>().SaveProgress(Progress);
        }

        public void SetProgress(PlayerProgress progress)
        {
            Progress = progress;
        }

        public int MakeMoney()
        {
            var coin = 222; 
            
            // Calculate
            
            return coin;
        }

        public void UpdateHud()
        {
            HUD.SetLevel(Progress.Level);
            HUD.SetCoin(Progress.Coin);
            HUD.SetDiamond(Progress.Diamond);
        }
    }
}