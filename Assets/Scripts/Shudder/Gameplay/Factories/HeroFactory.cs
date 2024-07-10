using Config;
using DI;
using Shudder.Gameplay.Configs;
using Shudder.Gameplay.Models;
using Shudder.Models;
using UnityEngine;

namespace Shudder.Gameplay.Factories
{
    public class HeroFactory
    {
        private readonly DIContainer _container;
        private readonly HeroConfig _heroConfig;

        public HeroFactory(DIContainer container, GameConfig gameConfig)
        {
            _container = container;
            _heroConfig = gameConfig.GetConfig<HeroConfig>();
        }
        
        public Hero Create(Ground[,] grounds)
        {
            var ground = grounds[_heroConfig.StartPositionX, _heroConfig.StartPositionY];
            var position = ground.AnchorPoint.position;

            var hero = new Hero(_container, _heroConfig.SpeedMove)
            {
                Health = 3,
            };
            
            var heroView = Object.Instantiate(_heroConfig.Prefab, position, Quaternion.identity, ground.AnchorPoint);
            heroView.Construct(_container);
            hero.ChangePosition(ground.AnchorPoint.position);
            hero.ChangeGround(ground);
            return hero;
        }
    }
}