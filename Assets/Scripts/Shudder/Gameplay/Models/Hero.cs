using Cysharp.Threading.Tasks;
using DI;
using Shudder.Events;
using Shudder.Gameplay.Models.Interfaces;
using Shudder.Gameplay.Presenters;
using Shudder.Gameplay.Services;
using Shudder.Models.Interfaces;
using UnityEngine;

namespace Shudder.Gameplay.Models
{
    public class Hero : IHero
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
        public float Speed { get; set;}
        public int Health { get; set;}
        public IGround CurrentGround { get; set; }
        public Vector3 Position { get; set; }

        public void Damage()
        {
            Health--;
        }

        public void ChangePosition(Vector3 position)
        {
            Position = position;
            _triggerEventBus.TriggerChangeHeroPosition(position);
        }

        public void ChangeGround(IGround ground)
        {
            _indicatorService.RemoveSelectIndicators();
            CurrentGround = ground;
            _triggerEventBus.TriggerChangeHeroParentGround(ground.AnchorPoint);
        }

        public async void SetGround(IGround ground)
        {
            CurrentGround = ground;
            
            await UniTask.Delay(500);
            _indicatorService.CreateSelectIndicators(ground);
        }

        public HeroPresenter Presenter { get; set; }
    }
}