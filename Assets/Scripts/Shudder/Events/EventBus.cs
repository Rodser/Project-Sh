using UnityEngine;
using UnityEngine.Events;

namespace Shudder.Events
{
    public class EventBus : IReadOnlyEventBus, ITriggerOnlyEventBus
    {
        public UnityEvent StartGameplayScene { get; } = new();
        public UnityEvent PlayGame { get; } = new();
        public UnityEvent PlayNextLevel { get; } = new();
        public UnityEvent<int> UpdateCoin { get; } = new();
        public UnityEvent UpdateUI { get; } = new();
        public UnityEvent<Transform> HasVictory { get; } = new();
        public UnityEvent OpenSettings { get; } = new();
        public UnityEvent RefreshLevel { get; } = new();
        public UnityEvent GoMenu { get; } = new();
        public UnityEvent OpenLeaderboards { get; } = new();
        public UnityEvent<float> MusicMute { get; } = new();
        public UnityEvent<float> SoundMute { get; } = new();
        public UnityEvent LevelToMenu { get; } = new();
        public UnityEvent TakeCoin { get; } = new();
        public UnityEvent DieHero { get; } = new();
        public UnityEvent ActivateSuperJump { get; } = new();

        public void UnSubscribe()
        {
            PlayGame.RemoveAllListeners();
            PlayNextLevel.RemoveAllListeners();
            HasVictory.RemoveAllListeners();
            StartGameplayScene.RemoveAllListeners();
            GoMenu.RemoveAllListeners();
            UpdateUI.RemoveAllListeners();
            OpenLeaderboards.RemoveAllListeners();
            ActivateSuperJump.RemoveAllListeners();
            Debug.Log("UnSubscribe");
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

        public void TriggerUpdateCoin(int value) => 
            UpdateCoin?.Invoke(value);

        public void TriggerUpdateUI() => 
            UpdateUI?.Invoke();

        public void TriggerOpenLeaderboards() => 
            OpenLeaderboards?.Invoke();

        public void TriggerMusicMute(float value) => 
            MusicMute?.Invoke(value);

        public void TriggerSoundMute(float value) => 
            SoundMute?.Invoke(value);

        public void TriggerLevelToMenu() => 
            LevelToMenu?.Invoke();

        public void TriggerTakeCoin() => 
            TakeCoin?.Invoke();

        public void TriggerDieHero() => 
            DieHero?.Invoke();

        public void TriggerActivateSuperJump() => 
            ActivateSuperJump?.Invoke();
    }
}