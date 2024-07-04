using System;
using Config;
using Core;
using DI;
using Logic;
using Rodser.Config;
using Shudder.Gameplay.Characters.Configs;
using Shudder.Gameplay.Characters.Factoryes;
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

            InitializeFactory();
            InitializeSystem();
            
            Game game = new Game();
            game.Run(_container, _gameConfig);
        }
        
        private void InitializeFactory()
        {
            _container.RegisterTransient(c => new BodyFactory());
            _container.RegisterTransient("MenuGrid",c => 
                new GridFactory(_gameConfig.GetConfig<HexogenGridConfig>()));
            _container.RegisterTransient(c => new SoundFactory(_gameConfig.GetConfig<SFXConfig>()));
            _container.RegisterTransient("LevelGrid",c => 
                new GridFactory(_gameConfig.LevelGridConfigs));
            _container.RegisterTransient(c => 
                new BallFactory(_gameConfig.GetConfig<BallConfig>(), _gameConfig.LevelGridConfigs));
            _container.RegisterSingleton(c => 
                new HeroFactory(_gameConfig.GetConfig<HeroConfig>(), _gameConfig.LevelGridConfigs));
            _container.RegisterTransient(c =>  new LightFactory());
        }

        private void InitializeSystem()
        {
            _container.RegisterSingleton(c => new CameraSystem(Camera.main));
            _container.RegisterSingleton(c => new InputSystem());
        }
    }
}