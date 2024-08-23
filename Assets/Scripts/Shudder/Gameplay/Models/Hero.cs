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
        private TriggerKeyView _triggerKeyView;

        public Hero(DIContainer container)
        {
            _container = container;
        }
        
        public IGround CurrentGround { get; set; }
        public HeroPresenter Presenter { get; set; }

        public async UniTask ChangeGround(IGround ground)
        {
            if(_indicatorService is not null)
                await _indicatorService.RemoveIndicators();
            Presenter.View.ChangeGround(ground.Presenter.View.transform);
            CurrentGround = ground;
        }

        public async void Construct(IGround ground, IndicatorService indicatorService)
        {
            CurrentGround = ground;
            _indicatorService = indicatorService;
            _triggerKeyView = Presenter.View.GetComponent<TriggerKeyView>();
            Presenter.View.transform.position = ground.AnchorPoint.position;
            if(indicatorService is not null)
                await indicatorService.CreateSelectIndicators(ground);
        }

        public void ActivateTriggerKew(GridConfig config)
        {
            _triggerKeyView.Activate(_container, config.IsKey);
        }
    }
}