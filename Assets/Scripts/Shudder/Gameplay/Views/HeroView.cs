using BaCon;
using Cysharp.Threading.Tasks;
using Shudder.Events;
using Shudder.Gameplay.Presenters;
using Shudder.Gameplay.Services;
using Shudder.Models;
using Shudder.Services;
using Shudder.Vews;
using UnityEngine;

namespace Shudder.Gameplay.Views
{
    [RequireComponent(typeof(Rigidbody))]
    public class HeroView : MonoBehaviour
    {
        private ITriggerOnlyEventBus _triggerEventBus;
        private ActivationPortalService _activationService;
        private SfxService _sfx;
        private bool _canUsePortal = false;

        public HeroPresenter Presenter { get; set; }

        public void Construct(DIContainer container, HeroPresenter presenter)
        {
            _triggerEventBus = container.Resolve<ITriggerOnlyEventBus>();
            _sfx = container.Resolve<SfxService>();
            
            Presenter = presenter;
            Presenter.SetView(this);
        }

        public void CanUsePortal()
        {
            _canUsePortal = true;
        }
        
        public void ChangeGround(Transform parentGround)
        {
            transform.SetParent(parentGround);
        }

        private void OnTriggerEnter(Collider other)
        {
            var groundView = other.GetComponentInParent<GroundView>();
            if (groundView is null)
                return;
            if (groundView.Presenter.Ground.GroundType != GroundType.Portal)
                return;
            if(!_canUsePortal)
                return;
            
            _sfx.InPortal();
            RunNewLevel(groundView.AnchorPoint);
        }

        private void RunNewLevel(Transform groundAnchorPoint)
        {
            Debug.Log("Victory!!! Run New Level");
            _canUsePortal = false;
            _triggerEventBus.TriggerVictory(groundAnchorPoint);
        }
    }
}