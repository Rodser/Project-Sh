using UnityEngine;
using UnityEngine.Events;

namespace Shudder.Events
{
    public class EventBus : IReadOnlyEventBus, ITriggerOnlyEventBus
    {
        public UnityEvent StartGameplayScene { get; } = new();
        public UnityEvent FlyCamera { get; } = new();
        public UnityEvent ExitGame { get; } = new();
        public UnityEvent<Vector3> ChangeHeroPosition { get; } = new();

        public void TriggerStartGameplayScene()
        {
            StartGameplayScene?.Invoke();
        }

        public void TriggerFlyCamera()
        {
            FlyCamera?.Invoke();
        }

        public void TriggerExitGame()
        {
            ExitGame?.Invoke();
        }

        void ITriggerOnlyEventBus.ChangeHeroPosition(Vector3 position)
        {
            ChangeHeroPosition?.Invoke(position);
        }
    }
}