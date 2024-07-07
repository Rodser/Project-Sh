using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DI;
using Model;
using Shudder.Gameplay.Characters.Models;
using Shudder.Gameplay.Characters.Views;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shudder.Gameplay.Services
{
    public class HeroMoveService
    {
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
            if(!TryGetSelectGround(out Ground selectGround))
                return;

            foreach (var groundNeighbor in _hero.CurrentGround.Neighbors
                         .Where(g => g.Id == selectGround.Id))
            {
                await MoveToTarget(groundNeighbor.AnchorPoint.position);
                _hero.ChangeGround(groundNeighbor);
                
                groundNeighbor.SwapWaveAsync(new List<Vector2>());
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
            if (!CheckGroundType(selectGround))
                return false;
            
            return true;
        }

        private bool CheckGroundType(Ground ground)
        {
            Debug.Log($"{ground.GroundType}");

            if (ground.GroundType == GroundType.TileHigh)
            {
                switch (_hero.CurrentGround.GroundType)
                {
                    case GroundType.TileHigh:
                    case GroundType.TileMedium:
                    case GroundType.Hole:
                        return true;
                }
            }
            else if (ground.GroundType == GroundType.TileLow)
            {
                switch (_hero.CurrentGround.GroundType)
                {
                    case GroundType.TileLow:
                    case GroundType.TileMedium:
                    case GroundType.Hole:
                        return true;
                }
            }

            return ground.GroundType == GroundType.TileMedium;
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