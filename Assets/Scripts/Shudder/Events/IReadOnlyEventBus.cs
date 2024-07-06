using UnityEngine.Events;

namespace Shudder.Events
{
    public interface IReadOnlyEventBus
    {
        public UnityEvent StartGameplayScene { get; }

    }
}