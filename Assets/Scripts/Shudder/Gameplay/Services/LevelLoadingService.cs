using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DI;
using Shudder.Configs;
using Shudder.Factories;
using Shudder.Gameplay.Factories;
using Shudder.Gameplay.Models;
using Shudder.Gameplay.Root;
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
            var level = game.CurrentLevel;
            Debug.Log($"Load Level {level}");

            var loadingScreenView = _container.Resolve<UIRootView>().LoadingScreenView;
            loadingScreenView.Report(0f);
            
            DestroyOld(game, loadingScreenView);

            var currentGrid = await CreateGrid(level, loadingScreenView);
            game.SetCurrentGrid(currentGrid);
            CreateLights(currentGrid, loadingScreenView);
            var hero = CreateHero(currentGrid, loadingScreenView);
            CreateActivatePortal(level, hero, currentGrid, loadingScreenView);
            hero.EnableIndicators();
            game.Hero = hero;
            
            var moveService = _container.Resolve<HeroMoveService>();
            moveService.Subscribe(hero);
            loadingScreenView.Report(1f);
        }

        private void CreateActivatePortal(int level, Hero hero, Grid currentGrid, LoadingScreenView loadingScreenView)
        {
            if (_gameConfig.LevelGridConfigs[level].IsKey)
            {
                hero.ActivateTriggerKew(_gameConfig.LevelGridConfigs[level]);
                _container.Resolve<ActivationPortalService>().Construct(currentGrid, _gameConfig.LevelGridConfigs[level].IsKey);
                
                _container
                    .Resolve<JewelKeyFactory>()
                    .Create(_gameConfig.JewelKeyView, _gameConfig.LevelGridConfigs[level], currentGrid);
            }

            loadingScreenView.Report(0.7f);
        }

        private Hero CreateHero(Grid currentGrid, LoadingScreenView loadingScreenView)
        {
            var hero = _container
                .Resolve<HeroFactory>()
                .Create(currentGrid.Grounds);
            loadingScreenView.Report(0.6f);
            return hero;
        }

        private void CreateLights(Grid currentGrid, LoadingScreenView loadingScreenView)
        {
            _container
                .Resolve<LightFactory>()
                .Create(_gameConfig.Light, currentGrid);
            loadingScreenView.Report(0.5f);
        }

        private async Task<Grid> CreateGrid(int level, LoadingScreenView loadingScreenView)
        {
            var currentGrid = await _container
                .Resolve<GridFactory>("LevelGrid")
                .Create(level);
            loadingScreenView.Report(0.4f);
            return currentGrid;
        }

        private static void DestroyOld(Game game, LoadingScreenView loadingScreenView)
        {
            game.DestroyGrid();
            loadingScreenView.Report(0.1f);
        }
    }
}