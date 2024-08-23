using BaCon;
using Shudder.Data;
using Shudder.Events;
using Shudder.Gameplay.Models.Interfaces;
using Shudder.Models;

namespace Shudder.Gameplay.Services
{
    public class SuperJumpService
    {
        private readonly IReadOnlyEventBus _readOnlyEvent;
        private readonly IndicatorService _indicatorService;
        private readonly StorageService _storage;
        
        private Ground[,] _grounds;
        private IHero _hero;

        public SuperJumpService(DIContainer container)
        {
            _indicatorService = container.Resolve<IndicatorService>();
            _storage = container.Resolve<StorageService>();
            _readOnlyEvent = container.Resolve<IReadOnlyEventBus>();
        }

        public bool IsActivationSuperJump { get; private set; }

        public void Init(Ground[,] grounds, IHero hero)
        {
            _grounds = grounds;
            _hero = hero;
            _readOnlyEvent.ActivateSuperJump.AddListener(ActivateSuperJump);
        }

        private async void ActivateSuperJump()
        {
            if(!TryDeductOfJumpCount())
                return;
            IsActivationSuperJump = true;
            await _indicatorService.RemoveIndicators();
            await _indicatorService.CreateAllIndicators(_grounds, _hero.CurrentGround);
        }

        private bool TryDeductOfJumpCount()
        {
            if (_storage.Progress.JumpCount <= 0)
                return false;
            
            _storage.DeductJumpCount();
            return true;
        }

        public void Jumped()
        {
            IsActivationSuperJump = false;
        }
    }
}