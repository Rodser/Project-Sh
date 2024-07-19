using Cysharp.Threading.Tasks;
using DI;
using Shudder.Events;
using Shudder.Gameplay.Presenters;
using Shudder.Models;
using Shudder.Vews;
using UnityEngine;

namespace Shudder.Gameplay.Views
{
    [RequireComponent(typeof(Rigidbody))]
    public class HeroView : MonoBehaviour
    {
        private ITriggerOnlyEventBus _triggerEventBus;

        public HeroPresenter Presenter { get; set; }

        public void Construct(DIContainer container, HeroPresenter presenter)
        {
            _triggerEventBus = container.Resolve<ITriggerOnlyEventBus>();
            var readEventBus = container.Resolve<IReadOnlyEventBus>();
            
            Presenter = presenter;
            Presenter.SetView(this);
            
            readEventBus.ChangeHeroPosition.AddListener(OnChangePosition);
            readEventBus.ChangeHeroParentGround.AddListener(OnChangeParent);
        }

        private void OnChangeParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        private void OnChangePosition(Vector3 position)
        {
            transform.position = position;
        }

        private void OnTriggerEnter(Collider other)
        {
            var ground = other.GetComponentInParent<GroundView>();
            if (ground is null)
                return;

            switch (ground.Presenter.Ground.GroundType)
            {
                case GroundType.Hole:
                    Debug.Log("Victory");
                    RunNewLevel(ground.AnchorPoint);
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
            await UniTask.Delay(500);
            _triggerEventBus.TriggerVictory(groundAnchorPoint);
        }
    }
}