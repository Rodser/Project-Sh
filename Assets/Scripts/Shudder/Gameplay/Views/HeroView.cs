using Cysharp.Threading.Tasks;
using DI;
using Shudder.Events;
using Shudder.Gameplay.Presenters;
using Shudder.Gameplay.Services;
using Shudder.Models;
using Shudder.Vews;
using UnityEngine;

namespace Shudder.Gameplay.Views
{
    [RequireComponent(typeof(Rigidbody))]
    public class HeroView : MonoBehaviour
    {
        private ITriggerOnlyEventBus _triggerEventBus;
        private bool _hasRunLevel = false;
        private ActivationPortalService _activationService;

        public HeroPresenter Presenter { get; set; }

        public void Construct(DIContainer container, HeroPresenter presenter)
        {
            _triggerEventBus = container.Resolve<ITriggerOnlyEventBus>();
            
            Presenter = presenter;
            Presenter.SetView(this);
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

            switch (groundView.Presenter.Ground.GroundType)
            {
                case GroundType.Portal:
                    Debug.Log("Victory");
                    RunNewLevel(groundView.AnchorPoint);
                    break;
                case GroundType.Pit:
                    Destroy(gameObject, 1000);
                    Debug.Log("Looser");
                    break;
                default:
                    return;
            }
        }

        private async void RunNewLevel(Transform groundAnchorPoint)
        {
            if(_hasRunLevel)
                return;
            
            _hasRunLevel = true;
            await UniTask.Delay(500);
            _triggerEventBus.TriggerVictory(groundAnchorPoint);
        }
    }
}