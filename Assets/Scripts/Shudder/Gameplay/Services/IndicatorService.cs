using System.Collections.Generic;
using System.Linq;
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
        private readonly List<Indicator> _boxSelectIndicators;
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
            foreach (var indicator in from neighbor in ground.Neighbors 
                     where _checkingPossibilityOfJump.CheckPossible(neighbor.GroundType, ground.GroundType)
                           && neighbor.GroundType != GroundType.Pit 
                     select Object.Instantiate(_gameConfig.SelectIndicator, neighbor.AnchorPoint)) 
                _boxSelectIndicators.Add(indicator);
        }
        
        public async UniTask RemoveSelectIndicators()
        {
            if (_boxSelectIndicators == null)
                return;
            foreach (var indicator in _boxSelectIndicators.Where(i => i is not null))
            {
                Object.Destroy(indicator.gameObject);
                await UniTask.Yield();
            }
            _boxSelectIndicators.Clear();
        }
    }
}