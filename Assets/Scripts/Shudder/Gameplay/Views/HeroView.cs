using Cysharp.Threading.Tasks;
using DI;
using Shudder.Events;
using Shudder.Gameplay.Models;
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

        public void Construct(DIContainer container)
        {
            _triggerEventBus = container.Resolve<ITriggerOnlyEventBus>();
            var readEventBus = container.Resolve<IReadOnlyEventBus>();
            
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