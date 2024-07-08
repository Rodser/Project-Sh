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
        public Ground CurrentGround { get; private set; }
        public Vector3 Position { get; private set; }

        public void Damage()
        {
            Health--;
        }

        public void ChangePosition(Vector3 position)
        {
            Position = position;
            var triggerEventBus = _container.Resolve<ITriggerOnlyEventBus>();
            triggerEventBus.TriggerChangeHeroPosition(position);
        }

        public void ChangeGround(Ground ground, Indicator prefab)
        {
            CurrentGround?.RemoveSelectIndicators();
            CurrentGround = ground;
            var triggerEventBus = _container.Resolve<ITriggerOnlyEventBus>();
            triggerEventBus.TriggerChangeHeroParentGround(ground.AnchorPoint);
            
            CurrentGround.CreateSelectIndicators(prefab);
        }
    }
}