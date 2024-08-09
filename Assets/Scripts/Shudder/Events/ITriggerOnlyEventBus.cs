using UnityEngine;

namespace Shudder.Events
{
    public interface ITriggerOnlyEventBus
    {
        void TriggerStartGameplayScene();
        void TriggerFlyCamera();
        void TriggerChangeHeroParentGround(Transform parent);
        void TriggerVictory(Transform groundAnchorPoint);
        void TriggerOpenSettings();
        void TriggerRefreshLevel();
        void TriggerGoMenu();
    }
}