using UnityEngine.Events;

namespace Shudder.Events
{
    public interface IReadOnlyEventBus
    {
        UnityEvent StartGameplayScene { get; }
        UnityEvent FlyCamera { get; }
        UnityEvent ExitGame { get; }
    }
}