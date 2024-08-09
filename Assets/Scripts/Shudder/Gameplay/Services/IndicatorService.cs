using System.Collections.Generic;
using System.Linq;
using BaCon;
using Shudder.Configs;
using Shudder.Gameplay.Models;
using Shudder.Models;
using Shudder.Models.Interfaces;
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

        public void CreateSelectIndicators(IGround ground)
        {
            if(ground.Neighbors == null)
                return;
            
            _boxSelectIndicators = new List<Indicator>();
            
            foreach (Ground neighbor in ground.Neighbors)
            {
                if (_container.Resolve<CheckingPossibilityOfJumpService>()
                    .CheckPossible(neighbor.GroundType, ground.GroundType)
                    && neighbor.GroundType != GroundType.Pit)
                {
                    var indicator = Object.Instantiate(_gameConfig.SelectIndicator, neighbor.AnchorPoint);
                    _boxSelectIndicators.Add(indicator);
                }
            }
        }
        
        public void RemoveSelectIndicators()
        {
            if(_boxSelectIndicators == null)
                return;
            
            foreach (var indicator in _boxSelectIndicators
                         .Where(i => i != null))
            {
                Object.Destroy(indicator.gameObject);
            }

            _boxSelectIndicators = null;
        }
    }
}