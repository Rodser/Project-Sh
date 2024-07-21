using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DI;
using Shudder.Gameplay.Configs;
using Shudder.Gameplay.Models.Interfaces;
using Shudder.Models.Interfaces;
using Shudder.Services;
using Shudder.Vews;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shudder.Gameplay.Services
{
    public class HeroMoveService
    {
        private readonly DIContainer _container;
        private readonly HeroConfig _heroConfig;
        private IHero _hero;
        private readonly JumpService _jumpService;

        public HeroMoveService(DIContainer container, HeroConfig heroConfig)
        {
            _container = container;
            _heroConfig = heroConfig;
            _jumpService = _container.Resolve<JumpService>();
        }

        public void Subscribe(IHero hero)
        {
            _hero = hero;
            _container.Resolve<InputService>().AddListener(Move);
        }

        private async void Move(InputAction.CallbackContext callback)
        {
            if (!TryGetSelectGround(out var selectGround))
                return;

            if (selectGround.Id == _hero.CurrentGround.Id)
            {
                await MoveHero(selectGround.Presenter.Ground);
                RunSwapWave(selectGround);
            }
            else
            {
                foreach (var groundNeighbor in _hero.CurrentGround.Neighbors
                             .Where(g => g.Id == selectGround.Id))
                {
                    await MoveHero(groundNeighbor);
                    RunSwapWave(groundNeighbor);
                }
            }
        }

        private async UniTask MoveHero(IGround ground)
        {
            _hero.ChangeGround(ground);
            await MoveToTarget(ground.AnchorPoint);
        }

        private async void RunSwapWave(IGround ground)
        {
            await _container
                .Resolve<SwapService>()
                .SwapWaveAsync(ground, new List<Vector2>(), true);

            _container.Resolve<IndicatorService>().CreateSelectIndicators(_hero.CurrentGround);
        }

        private bool TryGetSelectGround(out IGround selectGround)
        {
            var input = _container.Resolve<InputService>();
            var position = input.Position.ReadValue<Vector2>();
            var origin = _container.Resolve<CameraService>().Camera.ScreenPointToRay(position);

            Physics.Raycast(origin, out RaycastHit hit);
            if (hit.collider is null)
            {
                selectGround = null;
                return false;
            }

            selectGround = hit.collider.GetComponentInParent<GroundView>().Presenter.Ground;

            if (selectGround == null)
                return false;

            return _container
                .Resolve<CheckingPossibilityOfJumpService>()
                .CheckPossible(selectGround.GroundType, _hero.CurrentGround.GroundType);
        }

        private async UniTask MoveToTarget(Transform target)
        {
            await _jumpService.Jump(_heroConfig.JumpConfig, _hero.Presenter.View.transform, target);
        }
    }
}