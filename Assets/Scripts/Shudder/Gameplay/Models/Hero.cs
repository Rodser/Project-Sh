using BaCon;
using Cysharp.Threading.Tasks;
using Shudder.Configs;
using Shudder.Gameplay.Models.Interfaces;
using Shudder.Gameplay.Presenters;
using Shudder.Gameplay.Services;
using Shudder.Gameplay.Views;
using Shudder.Models.Interfaces;

namespace Shudder.Gameplay.Models
{
    public class Hero : IHero
    {
        private readonly DIContainer _container;
        
        private IndicatorService _indicatorService;

        public Hero(DIContainer container)
        {
            _container = container;
        }
        
        public IGround CurrentGround { get; set; }
        public HeroPresenter Presenter { get; set; }

        public void ChangeGround(IGround ground)
        {
            _indicatorService?.RemoveSelectIndicators();
            Presenter.View.ChangeGround(ground.Presenter.View.transform);
            CurrentGround = ground;
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

        public void ActivateTriggerKew(GridConfig config)
        {
            Presenter.View.GetComponent<TriggerKeyView>().Activate(_container, config.IsKey);
        }
    }
}