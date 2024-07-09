using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DI;
using Model;
using Shudder.Gameplay.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shudder.Gameplay.Services
{
    public class HeroMoveService
    {
        private const int SwapLimit = 5;
        
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
            if (TryGetSelectGround(out var selectGround))
            {
                foreach (var groundNeighbor in _hero.CurrentGround.Neighbors
                             .Where(g => g.Id == selectGround.Id))
                {
                    await MoveToTarget(groundNeighbor.AnchorPoint.position);
                    _hero.ChangeGround(groundNeighbor);

                    groundNeighbor.SwapWaveAsync(new List<Vector2>(), SwapLimit);
                }
            }
        }

        private bool TryGetSelectGround(out Ground selectGround)
        {
            var input = _container.Resolve<InputService>();
            var position = input.Position.ReadValue<Vector2>();
            var origin = _container.Resolve<CameraService>().Camera.ScreenPointToRay(position);

            Physics.Raycast(origin, out RaycastHit hit);
            selectGround = hit.collider.GetComponentInParent<Ground>();

            if (selectGround == null)
                return false;
            
            return _container
                .Resolve<CheckingPossibilityOfJumpService>()
                .CheckPossible(selectGround.GroundType, _hero.CurrentGround.GroundType);
        }
      
        private async UniTask MoveToTarget(Vector3 targetPosition)
        {
            var deviation = Vector3.Lerp(_hero.Position, targetPosition, 0.4f);
            deviation.z += 2f;
            await Fly(deviation ,targetPosition);
        }
        
        private async UniTask Fly(Vector3 deviation, Vector3 target)
        {
            Vector3 startPosition = _hero.Position; 
            var timeInFly = 0f;
            while (timeInFly < 1f)
            {
                float speedFlying = 0.7f;
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