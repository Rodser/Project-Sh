using DI;
using Model;
using Shudder.Events;
using UnityEngine;

namespace Shudder.Gameplay.Characters.Models
{
    public class Hero
    {
        private readonly DIContainer _container;

        public Hero(DIContainer container, float speed)
        {
            _container = container;
            Speed = speed;
            AtHole = false;
        }

        public bool AtHole { get; set; }
        public float Speed { get; private set;}
        public int Health { get; set;}
        public Ground CurrentGround { get; set; }

        public void Damage()
        {
            Health--;
        }

        public void Move(Vector3 position)
        {
            Debug.Log($"Hero Move position {position}");
            var triggerEventBus = _container.Resolve<ITriggerOnlyEventBus>();
            triggerEventBus.ChangeHeroPosition(position);
        }
    }
}