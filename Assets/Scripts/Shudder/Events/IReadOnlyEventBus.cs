using UnityEngine;
using UnityEngine.Events;

namespace Shudder.Events
{
    public interface IReadOnlyEventBus
    {
        UnityEvent StartGameplayScene { get; }
        UnityEvent FlyCamera { get; }
        UnityEvent ExitGame { get; }
        UnityEvent<Vector3> ChangeHeroPosition { get; }
        UnityEvent<Transform> ChangeHeroParentGround { get; }
        UnityEvent HasVictory { get; }
    }
}