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

        public SwapService(DIContainer container)
        {
            _container = container;
        }
        
        public async UniTask SwapWaveAsync(IGround ground, List<Vector2> offsetItems, int swapLimit, bool isHero)
        {
            if(swapLimit <= 0)
                return;
            
            if (offsetItems.Any(item => item == ground.Id))
                return;
            
            offsetItems.Add(ground.Id);
            Swap(ground, isHero);
            await UniTask.Delay(100);

            foreach (var neighbor in ground.Neighbors)
            {
                await SwapWaveAsync(neighbor, offsetItems, --swapLimit, false);
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