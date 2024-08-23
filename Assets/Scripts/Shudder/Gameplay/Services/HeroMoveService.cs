using System.Collections.Generic;
using BaCon;
using Cysharp.Threading.Tasks;
using Shudder.Gameplay.Configs;
using Shudder.Gameplay.Models.Interfaces;
using Shudder.Models;
using Shudder.Models.Interfaces;
using Shudder.Services;
using Shudder.Views;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shudder.Gameplay.Services
{
    public class HeroMoveService
    {
        private readonly HeroConfig _heroConfig;
        private readonly JumpService _jumpService;
        private readonly InputService _inputService;
        private readonly SfxService _sfxService;
        private readonly SwapService _swapService;
        private readonly IndicatorService _indicatorService;
        private readonly CameraService _cameraService;
        private readonly CheckingPossibilityOfJumpService _checkingPossibilityService;
        private readonly AnimationHeroService _animationHeroService;
        private readonly SuperJumpService _superJumpService;

        private IHero _hero;

        public HeroMoveService(DIContainer container, HeroConfig heroConfig)
        {
            _heroConfig = heroConfig;
            _jumpService = container.Resolve<JumpService>();
            _superJumpService = container.Resolve<SuperJumpService>();
            _inputService = container.Resolve<InputService>();
            _sfxService = container.Resolve<SfxService>();
            _swapService = container.Resolve<SwapService>();
            _indicatorService = container.Resolve<IndicatorService>();
            _cameraService = container.Resolve<CameraService>();
            _checkingPossibilityService = container.Resolve<CheckingPossibilityOfJumpService>();
            _animationHeroService = container.Resolve<AnimationHeroService>();
        }

        public void Subscribe(IHero hero)
        {
            _hero = hero;
            _inputService.AddListener(Move);
        }

        private async void Move(InputAction.CallbackContext callback)
        {
            if (!TryGetSelectGround(out var selectGround))
                return;
            if (_superJumpService.IsActivationSuperJump)
            {
                await MoveHero(selectGround);
                _superJumpService.Jumped();
                RunSwapWave(selectGround);
            }
            if (selectGround.Id == _hero.CurrentGround.Id)
            {
                // await MoveHero(selectGround.Presenter.Ground);
                RunSwapWave(selectGround);
            }
            else
            {
                for (int i = 0; i < _hero.CurrentGround.Neighbors.Count; i++)
                {
                    var neighbor = _hero.CurrentGround.Neighbors[i];
                    if (neighbor.Id != selectGround.Id)
                        continue;
                    
                    await MoveHero(neighbor);
                    RunSwapWave(neighbor);
                }
            }
        }

        private async UniTask MoveHero(IGround ground)
        {
            await _hero.ChangeGround(ground);
            await MoveToTarget(ground.AnchorPoint);
        }

        private async void RunSwapWave(IGround ground)
        {
            _sfxService.Thunder();
            
            await _swapService.SwapWaveAsync(ground, new List<Vector2>(), true);
            await _indicatorService.CreateSelectIndicators(_hero.CurrentGround);
        }

        private bool TryGetSelectGround(out IGround selectGround)
        {
            var position = _inputService.Position.ReadValue<Vector2>();
            var origin = _cameraService.Camera.ScreenPointToRay(position);

            Physics.Raycast(origin, out RaycastHit hit);
            if (hit.collider is null)
            {
                selectGround = null;
                return false;
            }

            selectGround = hit.collider.GetComponentInParent<GroundView>().Presenter.Ground;

            if (selectGround == null)
                return false;
            if(selectGround.GroundType == GroundType.Pit)
                return false;
            
            return _checkingPossibilityService.CheckPossible(selectGround.GroundType, _hero.CurrentGround.GroundType);
        }

        private async UniTask MoveToTarget(Transform target)
        {
            _sfxService.Jump();
            _animationHeroService.Jump();
            await _jumpService.Jump(_heroConfig.JumpConfig, _hero.Presenter.View.transform, target);
        }
    }
}