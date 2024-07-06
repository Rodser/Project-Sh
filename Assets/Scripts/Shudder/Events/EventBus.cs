using UnityEngine.Events;

namespace Shudder.Root
{
    public class EventBus
    {
        public UnityEvent StartGameplayScene { get; } = new();


        public void TriggerStartGameplayScene()
        {
            StartGameplayScene?.Invoke();
        }
    }
}