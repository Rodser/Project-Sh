using Core;
using Rodser.Config;
using Shudder.Gameplay.Characters.Configs;
using Shudder.Gameplay.Characters.Models;
using UnityEngine;

namespace Shudder.Gameplay.Characters.Factoryes
{
    public class HeroFactory
    {
        private readonly HeroConfig _heroConfig;
        private readonly HexogenGridConfig[] _hexGridConfigs;

        public HeroFactory(HeroConfig heroConfig, HexogenGridConfig[] hexGridConfigs)
        {
            _heroConfig = heroConfig;
            _hexGridConfigs = hexGridConfigs;
        }

        public Hero Create(Vector3 offsetPosition, int level, GameObject parent)
        {
            var position = GetStartPosition(level) + offsetPosition;

            var hero = new Hero(3, _heroConfig.SpeedMove);
            var heroView = Object.Instantiate(_heroConfig.Prefab, position, Quaternion.identity, parent.transform);
            heroView.Construct(hero);
            return hero;
        }

        private Vector3 GetStartPosition(int level)
        {
            var hexGridConfig = _hexGridConfigs[level];
            float x = _heroConfig.StartPositionX * hexGridConfig.SpaceBetweenCells;
            float y = _heroConfig.StartPositionY;
            float z = _heroConfig.StartPositionZ * hexGridConfig.SpaceBetweenCells;
            
            return new Vector3(x, y, z);
        }
    }
}