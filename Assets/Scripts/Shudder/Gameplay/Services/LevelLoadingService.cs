using Cysharp.Threading.Tasks;
using DI;
using Logic;
using Shudder.Configs;
using Shudder.Factories;
using Shudder.Gameplay.Configs;
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
            
            game.DieBody();
            game.Body = _container.Resolve<BodyFactory>().Create();
            
            _container
                .Resolve<LightFactory>()
                .Create(_gameConfig.Light, game.Camera.transform, game.Body.transform);
            
            var currentGrid = await _container
                .Resolve<GridFactory>("LevelGrid")
                .Create(level, game.Body.transform);

            var hero = _container
                .Resolve<HeroFactory>()
                .Create(currentGrid.Grounds);
            
            var moveService = _container.Resolve<HeroMoveService>();
            moveService.Subscribe(hero);
        }
    }
}