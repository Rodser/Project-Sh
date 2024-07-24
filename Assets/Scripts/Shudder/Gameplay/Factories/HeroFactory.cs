using DI;
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

        public HeroFactory(DIContainer container, HeroConfig heroConfig)
        {
            _container = container;
            _heroConfig = heroConfig;
        }
        
        public Hero Create(Ground[,] grounds)
        {
            var ground = grounds[_heroConfig.StartPositionX, _heroConfig.StartPositionY];
            if (ground.GroundType == GroundType.Pit)
            {
                foreach (var ng in ground.Neighbors)
                {
                    if (ng.GroundType != GroundType.Pit)
                        ground = ng;
                }
            }
            
            Debug.Log(ground.AnchorPoint);
            
            var hero = new Hero(_container, _heroConfig.SpeedMove)
            {
                Health = 3,
            };
            
            var position = ground.AnchorPoint.position;
            var heroView = Object.Instantiate(_heroConfig.Prefab, position, Quaternion.identity, ground.AnchorPoint);
            var presenter = new HeroPresenter(hero);
            heroView.Construct(_container, presenter);
            hero.SetGround(ground);
            return hero;
        }
    }
}