using UnityEngine.Events;

namespace Shudder.Events
{
    public class EventBus : IReadOnlyEventBus
    {
        public UnityEvent StartGameplayScene { get; } = new();

        public void TriggerStartGameplayScene()
        {
            StartGameplayScene?.Invoke();
        }
    }
}