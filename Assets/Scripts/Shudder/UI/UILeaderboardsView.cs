using Shudder.Events;
using Shudder.Services;
using UnityEngine;
using YG;

namespace Shudder.UI
{
    public class UILeaderboardsView : PopUpView
    {
        [field: SerializeField] public LeaderboardYG leaderboardYg { get; private set; }
        
        public new void Bind(ITriggerOnlyEventBus triggerOnlyEvent, InputService inputService)
        {
            base.Bind(triggerOnlyEvent, inputService);
            leaderboardYg.UpdateLB();
        }

        public void GoBack()
        {
            CloseWindow();
        }
    }
}