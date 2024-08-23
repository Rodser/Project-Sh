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

        public async UniTask CreateSelectIndicators(IGround ground)
        {
            if(ground.Neighbors == null)
                return;
            if(ground.GroundType == GroundType.Portal)
                return;

            await RemoveIndicators();
            foreach (var indicator in from neighbor in ground.Neighbors 
                     where _checkingPossibilityOfJump.CheckPossible(neighbor.GroundType, ground.GroundType)
                           && neighbor.GroundType != GroundType.Pit 
                     select Object.Instantiate(_gameConfig.SelectIndicator, neighbor.AnchorPoint)) 
                _boxSelectIndicators.Add(indicator.SetBox(_boxSelectIndicators));
            await UniTask.Yield();
        }

        public async UniTask CreateAllIndicators(Ground[,] grounds, IGround heroCurrentGround)
        {
            await RemoveIndicators();
            foreach (var ground in grounds)
            {
                if(ground.GroundType is GroundType.Pit)
                    continue;
                if (!_checkingPossibilityOfJump.CheckPossible(ground.GroundType, heroCurrentGround.GroundType))
                    continue;
                
                var indicator = Object.Instantiate(_gameConfig.SelectIndicator, ground.AnchorPoint);
                _boxSelectIndicators.Add(indicator.SetBox(_boxSelectIndicators));
            }

            await UniTask.Yield();
            
        }

        public async UniTask RemoveIndicators()
        {
            if (_boxSelectIndicators == null)
                return;
            
            for (int i = 0; i < _boxSelectIndicators.Count; i++)
            {
                Object.Destroy(_boxSelectIndicators[i].gameObject);
            }
            _boxSelectIndicators.Clear();
            await UniTask.Yield();
        }
    }
}