using Config;
using Cysharp.Threading.Tasks;
using DI;
using Logic;
using Shudder.Gameplay.Characters.Factories;
using Shudder.Gameplay.Root;
using Shudder.Gameplay.Services;
using UnityEngine;

namespace Shudder.Gameplay.Factories
{
    public class GameFactory
    {
        private readonly DIContainer _container;
        private readonly GameConfig _gameConfig;
        private Game _game;

        public GameFactory(DIContainer container, GameConfig gameConfig)
        {
            _container = container;
            _gameConfig = gameConfig;
        }

        public async void Create()
        {
            _game = new Game(_container)
            {
                Camera = _container.Resolve<CameraService>().Camera
            };

            await LoadLevelAsync(_game.CurrentLevel);
        }

        private async UniTask LoadLevelAsync(int level)
        {
            Debug.Log($"Load Level {level}");
            _game.DieBody();
            _game.Body =_container.Resolve<BodyFactory>().Create();
            
            _container
                .Resolve<LightFactory>()
                .Create(_gameConfig.Light, _game.Camera.transform, _game.Body.transform);
            
            var currentGrid = await _container
                .Resolve<GridFactory>("LevelGrid")
                .Create(level, _game.Body.transform);

            var hero = _container
                .Resolve<HeroFactory>()
                .Create(currentGrid.Grounds, level, _game.Body.gameObject);
            
            var moveService = _container.Resolve<HeroMoveService>();
            moveService.Subscribe(hero);
            _game.HeroPosition = hero.CurrentGround.transform.position; //TODO: dadly
            _game.Run();
        }
    }
}