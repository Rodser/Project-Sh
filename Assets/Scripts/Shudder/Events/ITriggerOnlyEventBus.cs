using UnityEngine;

namespace Shudder.Events
{
    public interface ITriggerOnlyEventBus
    {
        void TriggerStartGameplayScene();
        void TriggerFlyCamera();
        void TriggerExitGame();
        void ChangeHeroPosition(Vector3 position);
    }
}