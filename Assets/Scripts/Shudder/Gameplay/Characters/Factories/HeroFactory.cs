using Config;
using DI;
using Model;
using Shudder.Gameplay.Characters.Configs;
using Shudder.Gameplay.Characters.Models;
using Shudder.Gameplay.Characters.Views;
using UnityEngine;

namespace Shudder.Gameplay.Characters.Factories
{
    public class HeroFactory
    {
        private readonly DIContainer _container;
        private readonly HeroConfig _heroConfig;
        private readonly HexogenGridConfig[] _hexGridConfigs;

        public HeroFactory(DIContainer container, HeroConfig heroConfig, HexogenGridConfig[] hexGridConfigs)
        {
            _container = container;
            _heroConfig = heroConfig;
            _hexGridConfigs = hexGridConfigs;
        }
        
        public Hero Create(Ground[,] grounds, int level, GameObject parent)
        {
            var ground = grounds[_heroConfig.StartPositionX, _heroConfig.StartPositionY];
            var position = ground.transform.position;
            position.y += 0.5f;
            
            var hero = new Hero(_container, _heroConfig.SpeedMove)
            {
                Health = 3,
                CurrentGround = ground
            };
            
            var heroView = Object.Instantiate(_heroConfig.Prefab, position, Quaternion.identity, parent.transform);
            heroView.Construct(_container, hero);
            return hero;
        }
    }
}