using Cysharp.Threading.Tasks;
using DI;
using Model;
using Shudder.Events;
using Shudder.Gameplay.Characters.Models;
using UnityEngine;

namespace Shudder.Gameplay.Characters.Views
{
    [RequireComponent(typeof(Rigidbody))]
    public class HeroView : MonoBehaviour
    {
        private Vector3 _startPosition;
        private ITriggerOnlyEventBus _triggerEventBus;

        public Hero Hero;
        public Rigidbody Rigidbody { get; private set; }

        private void Start()
        {
            _startPosition = transform.position;
            Rigidbody = GetComponent<Rigidbody>();
        }

        public void Construct(DIContainer container, Hero hero)
        {
            Hero = hero;
            _triggerEventBus = container.Resolve<ITriggerOnlyEventBus>();
            container.Resolve<IReadOnlyEventBus>().ChangeHeroPosition.AddListener(OnChangePosition);
            container.Resolve<IReadOnlyEventBus>().ChangeHeroParentGround.AddListener(OnChangeParent);
        }

        private void OnChangeParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        private void OnChangePosition(Vector3 position)
        {
            transform.position = position;
        }

        private void ResetBallPosition()
        {
            Hero.Damage();
            Debug.Log(Hero.Health);
            transform.position = _startPosition;
        }

        private void OnTriggerEnter(Collider other)
        {
            Ground ground = other.GetComponentInParent<Ground>();
            if (ground == null)
                return;

            switch (ground.GroundType)
            {
                case GroundType.Hole:
                    Hero.AtHole = true;
                    Debug.Log("Victory");
                    RunNewLevel();
                    break;
                case GroundType.Pit when Hero.Health > 0:
                    ResetBallPosition();
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