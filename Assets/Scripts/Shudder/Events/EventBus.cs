using UnityEngine;
using UnityEngine.Events;

namespace Shudder.Events
{
    public class EventBus : IReadOnlyEventBus, ITriggerOnlyEventBus
    {
        public UnityEvent StartGameplayScene { get; } = new();
        public UnityEvent PlayGame { get; } = new();
        public UnityEvent PlayNextLevel { get; } = new();
        public UnityEvent UpdateCoin { get; } = new();
        public UnityEvent<Transform> HasVictory { get; } = new();
        public UnityEvent OpenSettings { get; } = new();
        public UnityEvent RefreshLevel { get; } = new();
        public UnityEvent GoMenu { get; } = new();
        
        public void UnSubscribe()
        {
            PlayGame.RemoveAllListeners();
            PlayNextLevel.RemoveAllListeners();
            HasVictory.RemoveAllListeners();
            StartGameplayScene.RemoveAllListeners();
            GoMenu.RemoveAllListeners();
            OpenSettings.RemoveAllListeners();
        }

        public void TriggerStartGameplayScene() => 
            StartGameplayScene?.Invoke();

        public void TriggerPlayGame() => 
            PlayGame?.Invoke();

        public void TriggerVictory(Transform groundAnchorPoint) =>
            HasVictory?.Invoke(groundAnchorPoint);

        public void TriggerOpenSettings() => 
            OpenSettings?.Invoke();

        public void TriggerRefreshLevel() => 
            RefreshLevel?.Invoke();

        public void TriggerGoMenu() => 
            GoMenu?.Invoke();

        public void TriggerPlayNextLevel() => 
            PlayNextLevel?.Invoke();

        public void TriggerUpdateCoin() => 
            UpdateCoin?.Invoke();
    }
}