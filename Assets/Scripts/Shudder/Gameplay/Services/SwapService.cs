using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DI;
using Shudder.Models;
using Shudder.Models.Interfaces;
using UnityEngine;

namespace Shudder.Gameplay.Services
{
    public class SwapService
    {
        private readonly DIContainer _container;
        
        private int _swapLimit = 6;

        public SwapService(DIContainer container)
        {
            _container = container;
        }

        public async UniTask SwapWaveAsync(IGround ground, List<Vector2> offsetItems, bool isHero)
        {
            if(isHero)
                _swapLimit = 5;
            
            if (offsetItems.Any(item => item == ground.Id))
                return;
            
            if (_swapLimit == 0)
            {
                ground.ToDestroy();
                return;
            }
            
            offsetItems.Add(ground.Id);
            Swap(ground, isHero);
            await UniTask.Delay(100);

            for (var i = 0; i < ground.Neighbors.Count; i++)
            {
                var neighbor = ground.Neighbors[i];
                _swapLimit--;
                if (_swapLimit < 0)
                    return;

                await SwapWaveAsync(neighbor, offsetItems, false);
            }
        }

        private void Swap(IGround ground, bool isHero)
        {
            var groundType = ground.GroundType;
            if (IsStationary(groundType))
                return;

            groundType -= 1;
            if (groundType < 0)
            {
                groundType = isHero ? GroundType.TileLow : GroundType.TileHigh;
            }
            ground.ChangeGroundType(groundType);

            _container.Resolve<LiftService>().MoveAsync(ground.Presenter.View, ground.OffsetPosition.y);
        }
        
        private bool IsStationary(GroundType groundType)
        {
            return groundType == GroundType.Pit || groundType == GroundType.Hole || groundType == GroundType.Wall;
        }
    }
}