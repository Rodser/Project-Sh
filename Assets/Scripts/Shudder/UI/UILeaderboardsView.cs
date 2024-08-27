using Shudder.Events;
using Shudder.Services;
using UnityEngine;
using YG;

namespace Shudder.UI
{
    public class UILeaderboardsView : PopUpView
    {
        private SfxService _sfxService;
        [field: SerializeField] public LeaderboardYG leaderboardYg { get; private set; }
        
        public void Bind(ITriggerOnlyEventBus triggerOnlyEvent, InputService inputService, SfxService sfxService)
        {
            base.Bind(triggerOnlyEvent, inputService);
            _sfxService = sfxService;
            leaderboardYg.UpdateLB();
        }

        public void GoBack()
        {
            _sfxService.Click();
            CloseWindow();
        }
    }
}