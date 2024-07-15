using Cysharp.Threading.Tasks;
using DI;
using Shudder.Events;
using Shudder.Gameplay.Services;
using Shudder.Models;
using UnityEngine;

namespace Shudder.Gameplay.Models
{
    public class Hero
    {
        private readonly ITriggerOnlyEventBus _triggerEventBus;
        private readonly IndicatorService _indicatorService;

        public Hero(DIContainer container, float speed)
        {
            Speed = speed;
            
            _triggerEventBus = container.Resolve<ITriggerOnlyEventBus>();
            _indicatorService = container.Resolve<IndicatorService>();
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
            _triggerEventBus.TriggerChangeHeroPosition(position);
        }

        public void ChangeGround(Ground ground)
        {
            _indicatorService.RemoveSelectIndicators();
            CurrentGround = ground;
            _triggerEventBus.TriggerChangeHeroParentGround(ground.AnchorPoint);
        }

        public async void SetGround(Ground ground)
        {
            CurrentGround = ground;
            
            await UniTask.Delay(500);
            _indicatorService.CreateSelectIndicators(ground);
        }
    }
}