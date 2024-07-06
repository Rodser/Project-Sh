using System.Collections.Generic;
using DI;
using Model;
using Shudder.Gameplay.Characters.Views;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shudder.Gameplay.Services
{
    public class HeroMoveService
    {
        private readonly DIContainer _container;
        private HeroView _heroView;

        public HeroMoveService(DIContainer container)
        {
            _container = container;
        }

        public void Subscribe(HeroView heroView)
        {
            _heroView = heroView;
            _container.Resolve<InputService>().AddListener(Move);
        }
        
        private void Move(InputAction.CallbackContext callback)
        {
            var input = _container.Resolve<InputService>();
            var position = input.Position.ReadValue<Vector2>();
            var origin = _container.Resolve<CameraService>().Camera.ScreenPointToRay(position);

            Physics.Raycast(origin, out RaycastHit hit);
            var ground = hit.collider.GetComponentInParent<Ground>();

            if (ground == null)
                return;
            if (!ground.Raised)
                return;

            List<Vector2> shifteds = new List<Vector2>();
            ground.SwapWaveAsync(shifteds);

            MoveToTarget(ground.transform.position);
        }

        private void MoveToTarget(Vector3 position)
        {
            if(_heroView.gameObject == null)
                Object.Destroy(_heroView.gameObject);
            
            var force = position - _heroView.transform.position;
            _heroView.Rigidbody.AddForce(force.normalized * _heroView.Hero.Speed, ForceMode.Impulse);
        }
    }
}