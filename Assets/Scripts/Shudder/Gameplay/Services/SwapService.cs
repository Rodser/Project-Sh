using System.Collections.Generic;
using System.Linq;
using BaCon;
using Cysharp.Threading.Tasks;
using Shudder.Data;
using Shudder.Gameplay.Models;
using Shudder.Models;
using Shudder.Models.Interfaces;
using Shudder.Services;
using UnityEngine;

namespace Shudder.Gameplay.Services
{
    public class SwapService
    {
        private readonly LiftService _liftService;
        private readonly IndicatorService _indicatorService;
        private readonly StorageService _storageService;
        private readonly SfxService _sfxService;
        
        private Hero _hero;

        public SwapService(DIContainer container)
        {
            _liftService = container.Resolve<LiftService>();
            _storageService = container.Resolve<StorageService>();
            _indicatorService = container.Resolve<IndicatorService>();
            _sfxService = container.Resolve<SfxService>();
        }

        public void Init(Hero hero)
        {
            _hero = hero;
        }

        public async UniTask SwapWaveAsync(IGround ground)
        {
            var swapGrounds = new List<IGround>();
            AddNeighborGroundToSwap(ground, swapGrounds);

            _sfxService.Thunder();
            await Swap(ground);
            foreach (var swapGround in swapGrounds)
            {
                await Swap(swapGround);
            }
        }

        public async void RunMegaWave()
        {
            if(!TryDeductOfWaveCount())
                return;

            await _indicatorService.RemoveIndicators();
            await MegaSwapWaveAsync(_hero.CurrentGround);
            await _indicatorService.CreateSelectIndicators(_hero.CurrentGround);
        }

        private async UniTask MegaSwapWaveAsync(IGround ground)
        {
            var swapGrounds = new List<IGround>();
            AddNeighborGroundToSwap(ground, swapGrounds);
            
            foreach (var neighbor in ground.Neighbors)
            {
                AddNeighborGroundToSwap(neighbor, swapGrounds);
            }

            _sfxService.Thunder();
            await Swap(ground);
            foreach (var swapGround in swapGrounds)
            {
                _sfxService.Thunder();
                await Swap(swapGround, true);
            }
        }

        private void AddNeighborGroundToSwap(IGround ground, List<IGround> swapGrounds)
        {
            foreach (var neighbor in ground.Neighbors
                         .Where(neighbor => !swapGrounds.Contains(neighbor))
                         .Where(neighbor => neighbor != _hero.CurrentGround)) 
                swapGrounds.Add(neighbor);
        }
        
        private bool TryDeductOfWaveCount()
        {
            if (_storageService.Progress.MegaWave <= 0)
                return false;
            
            _storageService.DeductWave();
            return true;
        }
        
        private async UniTask Swap(IGround ground, bool isMegaWave = false)
        {
            var groundType = ground.GroundType;
            if (IsStationary(groundType, isMegaWave))
                return;
            if (Random.value < 0.2f && ground != _hero.CurrentGround)
            {
                ground.ToDestroy();
                return;
            }

            groundType -= 1;
            if (groundType < 0)
            {
                groundType = isMegaWave ? GroundType.TileHigh : GroundType.TileLow;
            }
            ground.ChangeGroundType(groundType);

            await _liftService.MoveAsync(ground.Presenter.View, ground.OffsetPosition.y, false);
        }

        private static bool IsStationary(GroundType groundType,  bool isMegaWave = false)
        {
            if (isMegaWave)
                return groundType == GroundType.Pit || groundType == GroundType.Portal;
            
            return groundType == GroundType.Pit || groundType == GroundType.Portal || groundType == GroundType.Wall;
        }
    }
}