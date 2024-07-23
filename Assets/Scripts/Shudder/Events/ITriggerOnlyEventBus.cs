using UnityEngine;

namespace Shudder.Events
{
    public interface ITriggerOnlyEventBus
    {
        void TriggerStartGameplayScene();
        void TriggerFlyCamera();
        void TriggerExitGame();
        void TriggerChangeHeroParentGround(Transform parent);
        void TriggerVictory(Transform groundAnchorPoint);
    }
}