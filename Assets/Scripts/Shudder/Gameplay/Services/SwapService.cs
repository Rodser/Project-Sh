using System.Collections.Generic;
using System.Linq;
using BaCon;
using Cysharp.Threading.Tasks;
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
                _swapLimit = ground.Neighbors.Count;
            
            if (offsetItems.Any(item => item == ground.Id))
                return;
            
            if (_swapLimit == 0)
            {
                ground.ToDestroy();
                return;
            }
            
            offsetItems.Add(ground.Id);
            Swap(ground, isHero);
            
            for (var i = 0; i < ground.Neighbors.Count; i++)
            {
                var neighbor = ground.Neighbors[i];
                _swapLimit--;
                if (_swapLimit < 0)
                    return;
                
                if (_swapLimit == 0)
                {
                    if(Random.value < 0.5f)
                        neighbor.ToDestroy();
                    return;
                }
                
                Swap(neighbor, false);
                await UniTask.Delay(50);
            }
            
            // for (var i = 0; i < ground.Neighbors.Count; i++)
            // {
            //     var neighbor = ground.Neighbors[i];
            //     _swapLimit--;
            //     if (_swapLimit < 0)
            //         return;
            //
            //     await UniTask.Delay(500);
            //     await SwapWaveAsync(neighbor, offsetItems, false);
            // }
        }

        private void Swap(IGround ground, bool isHero)
        {
            var groundType = ground.GroundType;
            if (IsStationary(groundType))
                return;

            groundType -= 1;
            if (groundType < 0)
            {
                groundType = 0;
                // groundType = isHero ? GroundType.TileLow : GroundType.TileHigh;
            }
            ground.ChangeGroundType(groundType);

            _container.Resolve<LiftService>().MoveAsync(ground.Presenter.View, ground.OffsetPosition.y, false);
        }
        
        private static bool IsStationary(GroundType groundType)
        {
            return groundType == GroundType.Pit || groundType == GroundType.Portal || groundType == GroundType.Wall;
        }
    }
}