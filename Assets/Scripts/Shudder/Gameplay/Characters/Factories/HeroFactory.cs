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
        private readonly Indicator _selectIndicator;

        public HeroFactory(DIContainer container, GameConfig gameConfig)
        {
            _container = container;
            _heroConfig = gameConfig.GetConfig<HeroConfig>();
            _selectIndicator = gameConfig.SelectIndicator;
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
            heroView.Construct(_container, hero);
            hero.ChangeGround(ground, _selectIndicator);
            hero.ChangePosition(ground.AnchorPoint.position);
            return hero;
        }
    }
}