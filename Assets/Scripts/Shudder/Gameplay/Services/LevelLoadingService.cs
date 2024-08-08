using Config;
using Cysharp.Threading.Tasks;
using DI;
using Shudder.Configs;
using Shudder.Data;
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

        public LevelLoadingService(DIContainer container, GameConfig gameConfig)
        {
            _container = container;
            _gameConfig = gameConfig;
        }

        public async UniTask LoadAsync(Game game)
        {
            var progress = _container.Resolve<StorageService>().LoadProgress();
            game.SetProgress(progress);
            var level = game.Progress.Level - 1;
            Debug.Log($"Load Level {level}");
            game.DestroyGrid();

            var currentGrid = await CreateGrid(level);
            game.SetCurrentGrid(currentGrid);
            CreateLights(currentGrid);
            CreateItems(currentGrid);
            CreateMusic(currentGrid);

            CreateHud(progress);
            var hero = CreateHero(currentGrid);
            CreateActivatePortal(level, hero, currentGrid);
            hero.EnableIndicators();
            game.Hero = hero;
            
            var moveService = _container.Resolve<HeroMoveService>();
            moveService.Subscribe(hero);
        }

        private void CreateHud(PlayerProgress progress)
        {
            var prefab = _gameConfig.HudView;
            var hudView = Object.Instantiate(prefab);
            hudView.SetLevel(progress.Level);
            hudView.SetCoin(progress.Coin);
            hudView.SetDiamond(progress.Diamond);
            _container.Resolve<UIRootView>().AttachSceneUI(hudView.gameObject);
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