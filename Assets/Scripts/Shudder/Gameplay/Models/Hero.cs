using Cysharp.Threading.Tasks;
using DI;
using Shudder.Events;
using Shudder.Gameplay.Models.Interfaces;
using Shudder.Gameplay.Presenters;
using Shudder.Gameplay.Services;
using Shudder.Models.Interfaces;

namespace Shudder.Gameplay.Models
{
    public class Hero : IHero
    {
        private readonly DIContainer _container;
        private readonly ITriggerOnlyEventBus _triggerEventBus;
        
        private IndicatorService _indicatorService;

        public Hero(DIContainer container, float speed)
        {
            _container = container;
            Speed = speed;
            
            _triggerEventBus = container.Resolve<ITriggerOnlyEventBus>();
        }
        
        public float Speed { get; set;}
        public int Health { get; set;}
        public IGround CurrentGround { get; set; }
        public HeroPresenter Presenter { get; set; }

        public void Damage()
        {
            Health--;
        }

        public void ChangeGround(IGround ground)
        {
            _indicatorService?.RemoveSelectIndicators();
            Presenter.View.ChangeGround(ground.Presenter.View.transform);
            CurrentGround = ground;
            _triggerEventBus.TriggerChangeHeroParentGround(ground.AnchorPoint);
        }

        public async void SetGround(IGround ground)
        {
            CurrentGround = ground;
            Presenter.View.transform.position = ground.AnchorPoint.position;
            await UniTask.Delay(500);
            _indicatorService?.CreateSelectIndicators(ground);
        }

        public void EnableIndicators()
        {
            _indicatorService = _container.Resolve<IndicatorService>();
        }
    }
}