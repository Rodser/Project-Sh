using UnityEngine;
using UnityEngine.Events;

namespace Shudder.Events
{
    public interface IReadOnlyEventBus
    {
        UnityEvent StartGameplayScene { get; }
        UnityEvent FlyCamera { get; }
        UnityEvent ExitGame { get; }
        UnityEvent<Transform> ChangeHeroParentGround { get; }
        UnityEvent<Transform> HasVictory { get; }
        UnityEvent OpenSettings { get; }
        UnityEvent RefreshLevel { get; }
        UnityEvent GoMenu { get; }
    }
}