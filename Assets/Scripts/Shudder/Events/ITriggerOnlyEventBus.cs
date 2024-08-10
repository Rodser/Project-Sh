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
    }
}