using Shudder.Events;
using Shudder.Services;
using UnityEngine;
using YG;

namespace Shudder.UI
{
    public class UILeaderboardsView : PopUpView
    {
        [SerializeField] private LeaderboardYG _leaderboardYg;
        
        public new void Bind(ITriggerOnlyEventBus triggerOnlyEvent, InputService inputService)
        {
            base.Bind(triggerOnlyEvent, inputService);
            YandexGame.GetLeaderboard(
                _leaderboardYg.nameLB,
                _leaderboardYg.maxQuantityPlayers,
                _leaderboardYg.quantityTop,
                _leaderboardYg.quantityAround,
                "small");
        }

        public void GoBack()
        {
            CloseWindow();
        }
    }
}