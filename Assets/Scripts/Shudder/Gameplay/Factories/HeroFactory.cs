using DI;
using Shudder.Configs;
using Shudder.Gameplay.Configs;
using Shudder.Gameplay.Models;
using Shudder.Gameplay.Presenters;
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
            var presenter = new HeroPresenter(hero);
            heroView.Construct(_container, presenter);
            hero.ChangePosition(ground.AnchorPoint.position);
            hero.SetGround(ground);
            return hero;
        }
    }
}