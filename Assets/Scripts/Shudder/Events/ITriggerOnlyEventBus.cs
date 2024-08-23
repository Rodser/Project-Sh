using UnityEngine;

namespace Shudder.Events
{
    public interface ITriggerOnlyEventBus
    {
        void TriggerStartGameplayScene();
        void TriggerPlayGame();
        void TriggerVictory(Transform groundAnchorPoint);
        void TriggerOpenSettings();
        void TriggerRefreshLevel();
        void TriggerGoMenu();
        void TriggerPlayNextLevel();
        void TriggerUpdateCoin(int value);
        void TriggerUpdateUI();
        void TriggerOpenLeaderboards();
        void TriggerMusicMute(float value);
        void TriggerSoundMute(float value);
        void TriggerLevelToMenu();
        void TriggerTakeCoin();
        void TriggerDieHero();
        void TriggerActivateSuperJump();
    }
}