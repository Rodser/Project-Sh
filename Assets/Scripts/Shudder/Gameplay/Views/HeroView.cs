using Cysharp.Threading.Tasks;
using DI;
using Model;
using Shudder.Events;
using Shudder.Gameplay.Models;
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
            Ground ground = other.GetComponentInParent<Ground>();
            if (ground is null)
                return;

            switch (ground.GroundType)
            {
                case GroundType.Hole:
                    Debug.Log("Victory");
                    RunNewLevel();
                    break;
                case GroundType.Pit:
                    Destroy(gameObject, 1000);
                    Debug.Log("Looser");
                    break;
            }
        }

        private async void RunNewLevel()
        {
            await UniTask.Delay(500);
            _triggerEventBus.TriggerVictory();
        }
    }
}