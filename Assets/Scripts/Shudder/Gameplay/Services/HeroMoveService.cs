using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DI;
using Shudder.Gameplay.Models;
using Shudder.Models;
using Shudder.Models.Interfaces;
using Shudder.Services;
using Shudder.Vews;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shudder.Gameplay.Services
{
    public class HeroMoveService
    {
        private const int SwapLimit = 6;
        
        private readonly DIContainer _container;
        private Hero _hero;

        public HeroMoveService(DIContainer container)
        {
            _container = container;
        }

        public void Subscribe(Hero hero)
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
                MoveHero(selectGround.Presenter.Ground);
                RunSwapWave(selectGround);
            }
            else
            {
                foreach (var groundNeighbor in _hero.CurrentGround.Neighbors
                             .Where(g => g.Id == selectGround.Id || selectGround.Id == _hero.CurrentGround.Id))
                {
                    MoveHero(groundNeighbor);
                    RunSwapWave(groundNeighbor);
                }
            }
        }

        private async void MoveHero(IGround ground)
        {
            await MoveToTarget(ground.AnchorPoint.position);
            _hero.ChangeGround(ground);
        }

        private async void RunSwapWave(IGround ground)
        {
            await _container
                .Resolve<SwapService>()
                .SwapWaveAsync(ground, new List<Vector2>(), SwapLimit, true);
                
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
      
        private async UniTask MoveToTarget(Vector3 targetPosition)
        {
            var deviation = Vector3.Lerp(_hero.Position, targetPosition, 0.5f);
            deviation.y += 1f;
            await Fly(deviation ,targetPosition);
        }
        
        private async UniTask Fly(Vector3 deviation, Vector3 target)
        {
            Vector3 startPosition = _hero.Position; 
            var timeInFly = 0f;
            while (timeInFly < 1f)
            {
                float speedFlying = 1f;
                timeInFly += speedFlying * Time.deltaTime;
                var position = GetCurve(startPosition, deviation, target, timeInFly);
                _hero.ChangePosition(position);
                await UniTask.Yield();
            }
        }

        private Vector3 GetCurve(Vector3 a, Vector3 b, Vector3 c, float time)
        {
            Vector3 ab = Vector3.Lerp(a, b, time);
            Vector3 bc = Vector3.Lerp(b, c, time);

            return Vector3.Lerp(ab, bc, time);
        }
    }
}