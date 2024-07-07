using Config;
using Core;
using DI;
using Logic;
using Shudder.Gameplay.Characters.Configs;
using Shudder.Gameplay.Characters.Factories;
using Shudder.Gameplay.Factories;
using Shudder.Gameplay.Services;
using UnityEngine;

namespace Shudder.Gameplay.Root
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        [SerializeField] private GameConfig _gameConfig = null;
        private DIContainer _container;

        public void Initialisation(DIContainer container)
        {
            _container = new DIContainer(container);

            InitializeFactories();
            InitializeServices();

            var gameFactory = new GameFactory(_container, _gameConfig);
            gameFactory.Create();
        }
        
        private void InitializeFactories()
        {
            _container.RegisterSingleton("LevelGrid",c => 
                new GridFactory(_gameConfig.LevelGridConfigs));
            _container.RegisterSingleton(c => new BodyFactory());
            _container.RegisterSingleton(c => 
                new HeroFactory(_container, _gameConfig.GetConfig<HeroConfig>(), _gameConfig.LevelGridConfigs));
            _container.RegisterSingleton(c => new LightFactory());
            _container.RegisterSingleton(c => new SoundFactory(_gameConfig.GetConfig<SFXConfig>()));
        }

        private void InitializeServices()
        {       
            _container.RegisterSingleton(c => new CameraService(Camera.main)); 
            _container.RegisterSingleton(c => new HeroMoveService(_container));
        }
    }
}