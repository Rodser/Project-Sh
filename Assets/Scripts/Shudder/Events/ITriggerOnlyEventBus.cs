using UnityEngine;

namespace Shudder.Events
{
    public interface ITriggerOnlyEventBus
    {
        void TriggerStartGameplayScene();
        void TriggerFlyCamera();
        void TriggerExitGame();
        void TriggerChangeHeroPosition(Vector3 position);
        void TriggerChangeHeroParentGround(Transform parent);
    }
}