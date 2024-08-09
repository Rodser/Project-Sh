using UnityEngine;
using UnityEngine.Events;

namespace Shudder.Events
{
    public class EventBus : IReadOnlyEventBus, ITriggerOnlyEventBus
    {
        public UnityEvent StartGameplayScene { get; } = new();
        public UnityEvent FlyCamera { get; } = new();
        public UnityEvent ExitGame { get; } = new();
        public UnityEvent<Transform> ChangeHeroParentGround { get; } = new();
        public UnityEvent<Transform> HasVictory { get; } = new();
        public UnityEvent OpenSettings { get; } = new();
        public UnityEvent RefreshLevel { get; } = new();
        public UnityEvent GoMenu { get; } = new();

        public void TriggerStartGameplayScene() => 
            StartGameplayScene?.Invoke();

        public void TriggerFlyCamera() => 
            FlyCamera?.Invoke();

        public void TriggerExitGame() => 
            ExitGame?.Invoke();

        public void TriggerChangeHeroParentGround(Transform parent) => 
            ChangeHeroParentGround?.Invoke(parent);

        public void TriggerVictory(Transform groundAnchorPoint) =>
            HasVictory?.Invoke(groundAnchorPoint);

        public void TriggerOpenSettings()
        {
            OpenSettings?.Invoke();
        }

        public void TriggerRefreshLevel()
        {
            RefreshLevel?.Invoke();
        }

        public void TriggerGoMenu()
        {
            GoMenu?.Invoke();
        }
    }
}