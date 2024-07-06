using UnityEngine.Events;

namespace Shudder.Events
{
    public class EventBus : IReadOnlyEventBus, ITriggerOnlyEventBus
    {
        public UnityEvent StartGameplayScene { get; } = new();
        public UnityEvent FlyCamera { get; } = new();
        public UnityEvent ExitGame { get; } = new();

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
    }
}