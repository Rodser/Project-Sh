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
        public UnityEvent<Transform> ChangeHeroParentGround { get; } = new();
        public UnityEvent<Transform> HasVictory { get; } = new();

        public void TriggerStartGameplayScene() => 
            StartGameplayScene?.Invoke();

        public void TriggerFlyCamera() => 
            FlyCamera?.Invoke();

        public void TriggerExitGame() => 
            ExitGame?.Invoke();

        public void TriggerChangeHeroPosition(Vector3 position) => 
            ChangeHeroPosition?.Invoke(position);

        public void TriggerChangeHeroParentGround(Transform parent) => 
            ChangeHeroParentGround?.Invoke(parent);

        public void TriggerVictory(Transform groundAnchorPoint) =>
            HasVictory?.Invoke(groundAnchorPoint);
    }
}