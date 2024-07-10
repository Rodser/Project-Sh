using System.Collections.Generic;
using DI;
using Shudder.Gameplay.Configs;
using Shudder.Gameplay.Models;
using Shudder.Models;
using UnityEngine;

namespace Shudder.Gameplay.Services
{
    public class IndicatorService
    {
        private readonly DIContainer _container;
        private readonly GameConfig _gameConfig;
        
        private List<Indicator> _boxSelectIndicators;

        public IndicatorService(DIContainer container, GameConfig gameConfig)
        {
            _container = container;
            _gameConfig = gameConfig;
        }


        public void CreateSelectIndicators(Ground ground)
        {
            _boxSelectIndicators = new List<Indicator>();
            
            foreach (Ground neighbor in ground.Neighbors)
            {
                if (_container.Resolve<CheckingPossibilityOfJumpService>()
                    .CheckPossible(neighbor.GroundType, ground.GroundType))
                {
                    var indicator = Object.Instantiate(_gameConfig.SelectIndicator, neighbor.AnchorPoint);
                    _boxSelectIndicators.Add(indicator);
                }
            }
        }
        
        public void RemoveSelectIndicators()
        {
            foreach (Indicator indicator in _boxSelectIndicators)
            { 
                Object.Destroy(indicator.gameObject);
            }

            _boxSelectIndicators = null;
        }
    }
}