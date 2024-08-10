using BaCon;
using Config;
using Cysharp.Threading.Tasks;
using Shudder.Configs;
using Shudder.Data;
using Shudder.Events;
using Shudder.Factories;
using Shudder.Gameplay.Factories;
using Shudder.Gameplay.Models;
using Shudder.Gameplay.Root;
using Shudder.Services;
using Shudder.UI;
using UnityEngine;
using Grid = Shudder.Models.Grid;

namespace Shudder.Gameplay.Services
{
    public class LevelLoadingService
    {
        private readonly DIContainer _container;
        private readonly GameConfig _gameConfig;
        private Game _game;

        public LevelLoadingService(DIContainer container, GameConfig gameConfig)
        {
            _container = container;
            _gameConfig = gameConfig;
        }

        public void Init(Game game)
        {
            _game = game;
            var progress = _container.Resolve<StorageService>().LoadProgress();
            _game.SetProgress(progress);
        }

        public async UniTask DestroyLevelAsync()
        {
            _container.Resolve<IReadOnlyEventBus>().HasVictory.RemoveListener(OnHasVictory);
            await _game.DestroyGrid();
        }

        public async UniTask LoadAsync()
        {
            var level = _game.Progress.Level;
            var currentGrid = await CreateGrid(level);
            _game.SetCurrentGrid(currentGrid);
            CreateLights(currentGrid);
            CreateItems(currentGrid);
            CreateMusic(currentGrid);
            CreateHud(_game.Progress);
            
            var hero = CreateHero(currentGrid);
            CreateActivatePortal(level, hero, currentGrid);
            hero.EnableIndicators();
            _game.Hero = hero;
            var moveService = _container.Resolve<HeroMoveService>();
            moveService.Subscribe(hero);
            
            Debug.Log($"Load Level progress {_container.Resolve<StorageService>().LoadProgress().Level}");
            _container.Resolve<IReadOnlyEventBus>().HasVictory.AddListener(OnHasVictory);
            _game.Run();
        }
        
        private async void OnHasVictory(Transform groundAnchorPoint)
        {
            _container.Resolve<CameraSurveillanceService>().UnFollow();
            var coin = LevelUp();
            _container.Resolve<VictoryHandlerService>().OpenVictoryWindow(coin, groundAnchorPoint);
        }

        private int LevelUp()
        {
            var oldCoin = _game.Progress.Coin;
            _game.UpLevel();
            var newCoiin = _game.Progress.Coin;
            return newCoiin - oldCoin;
        }

        private void CreateHud(PlayerProgress progress)
        {
            var prefab = _gameConfig.HudView;
            var hudView = Object.Instantiate(prefab);
            hudView.Bind(_container.Resolve<ITriggerOnlyEventBus>());
            hudView.SetLevel(progress.Level);
            hudView.SetCoin(progress.Coin);
            hudView.SetDiamond(progress.Diamond);
            _container.Resolve<UIRootView>().ChangeSceneUI(hudView.gameObject);
        }

        private void CreateMusic(Grid currentGrid)
        {
           var service = _container
                .Resolve<SfxService>();
           service.CreateMusic(_gameConfig.GetConfig<SFXConfig>(), currentGrid.Presenter.View.transform);
           service.StartMusic();
        }

        private void CreateItems(Grid currentGrid)
        {
            _container.Resolve<ItemFactory>().Create(_gameConfig.Items, currentGrid, 0.7f);
        }

        private void CreateActivatePortal(int level, Hero hero, Grid currentGrid)
        {
            if (!_gameConfig.LevelGridConfigs[level].IsKey)
                return;
            
            hero.ActivateTriggerKew(_gameConfig.LevelGridConfigs[level]);
            _container.Resolve<ActivationPortalService>().Construct(currentGrid, _gameConfig.LevelGridConfigs[level].IsKey);
                
            _container
                .Resolve<JewelKeyFactory>()
                .Create(_gameConfig.JewelKeyView, _gameConfig.LevelGridConfigs[level], currentGrid);
        }

        private Hero CreateHero(Grid currentGrid)
        {
            var hero = _container
                .Resolve<HeroFactory>()
                .Create(currentGrid.Grounds);
            return hero;
        }

        private void CreateLights(Grid currentGrid)
        {
            _container
                .Resolve<LightFactory>()
                .Create(_gameConfig.Lights, currentGrid, 0.2f);
        }

        private async UniTask<Grid> CreateGrid(int level)
        {
            var currentGrid = await _container
                .Resolve<GridFactory>("LevelGrid")
                .Create(level);
            return currentGrid;
        }
    }
}