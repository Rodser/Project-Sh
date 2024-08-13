using System.Collections.Generic;
using BaCon;
using Cysharp.Threading.Tasks;
using Shudder.Configs;
using Shudder.Gameplay.Models;
using Shudder.Models;
using Shudder.Models.Interfaces;
using UnityEngine;

namespace Shudder.Gameplay.Services
{
    public class IndicatorService
    {
        private readonly GameConfig _gameConfig;
        
        private List<Indicator> _boxSelectIndicators;
        private readonly CheckingPossibilityOfJumpService _checkingPossibilityOfJump;

        public IndicatorService(DIContainer container, GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
            _checkingPossibilityOfJump = container.Resolve<CheckingPossibilityOfJumpService>();
            _boxSelectIndicators = new List<Indicator>();
        }

        public async void CreateSelectIndicators(IGround ground)
        {
            if(ground.Neighbors == null)
                return;

            await RemoveSelectIndicators();
            foreach (Ground neighbor in ground.Neighbors)
            {
                if (_checkingPossibilityOfJump.CheckPossible(neighbor.GroundType, ground.GroundType)
                    && neighbor.GroundType != GroundType.Pit)
                {
                    var indicator = Object.Instantiate(_gameConfig.SelectIndicator, neighbor.AnchorPoint);
                    _boxSelectIndicators.Add(indicator);
                }
            }
        }
        
        public async UniTask RemoveSelectIndicators()
        {
            if (_boxSelectIndicators == null)
                return;
            for (var i = 0; i < _boxSelectIndicators.Count; i++)
            {
                var indicator = _boxSelectIndicators[i];
                if (indicator is null)
                    continue;
                
                //_boxSelectIndicators.Remove(indicator);
                Object.Destroy(indicator.gameObject);
                await UniTask.Yield();
            }
            _boxSelectIndicators.Clear();
        }
    }
}