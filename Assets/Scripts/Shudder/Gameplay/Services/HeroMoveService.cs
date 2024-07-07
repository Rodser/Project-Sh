using System.Collections.Generic;
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
        
        private void Move(InputAction.CallbackContext callback)
        {
            var input = _container.Resolve<InputService>();
            var position = input.Position.ReadValue<Vector2>();
            var origin = _container.Resolve<CameraService>().Camera.ScreenPointToRay(position);

            Physics.Raycast(origin, out RaycastHit hit);
            var selectGround = hit.collider.GetComponentInParent<Ground>();

            if (selectGround == null)
                return;
            if (!selectGround.Raised)
                return;

            foreach (Ground groundNeighbor in _hero.CurrentGround.Neighbors)
            {
                if (selectGround.Id == groundNeighbor.Id)
                {
                    MoveToTarget(selectGround.transform.position);
                    _hero.CurrentGround = selectGround;

                    selectGround.SwapWaveAsync(new List<Vector2>());
                }
            }
        }

        private void MoveToTarget(Vector3 position)
        {
            position.y += 0.5f;

            _hero.Move(position);
        }
    }
}