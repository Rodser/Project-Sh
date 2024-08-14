using BaCon;
using Shudder.Events;
using Shudder.Gameplay.Presenters;
using Shudder.Gameplay.Services;
using Shudder.Services;
using Shudder.Views;
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

        private void OnDestroy()
        {
            _triggerEventBus.TriggerDieHero();
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
            if (other.transform.parent.parent.TryGetComponent(out PortalView portalView))
            {
                Debug.Log("Portal");
                if (!_canUsePortal)
                    return;
                var groundView = portalView.GetComponentInParent<GroundView>();
                _sfx.InPortal();
                RunNewLevel(groundView.AnchorPoint);
            }
            else if(other.transform.parent.TryGetComponent(out CoinView coinView))
            {
                Debug.Log("Take Coin");
                _triggerEventBus.TriggerTakeCoin();
                coinView.TakeCoin();
            }
        }

        private void RunNewLevel(Transform groundAnchorPoint)
        {
            Debug.Log("Victory!!! Run New Level");
            _canUsePortal = false;
            _triggerEventBus.TriggerVictory(groundAnchorPoint);
            gameObject.SetActive(false);
        }
    }
}