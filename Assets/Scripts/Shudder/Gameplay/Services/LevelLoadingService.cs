using Cysharp.Threading.Tasks;
using DI;
using Shudder.Configs;
using Shudder.Factories;
using Shudder.Gameplay.Factories;
using Shudder.Gameplay.Root;
using UnityEngine;

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
            
            game.DestroyGrid();
            
            var currentGrid = await _container
                .Resolve<GridFactory>("LevelGrid")
                .Create(level);

            game.SetCurrentGrid(currentGrid);

            _container
                .Resolve<LightFactory>()
                .Create(_gameConfig.Light, currentGrid);

            var hero = _container
                .Resolve<HeroFactory>()
                .Create(currentGrid.Grounds);
            
            var moveService = _container.Resolve<HeroMoveService>();
            moveService.Subscribe(hero);
        }
    }
}