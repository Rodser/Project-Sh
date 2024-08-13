using BaCon;
using Shudder.Events;
using Shudder.MainMenu.Configs;
using Shudder.UI;
using UnityEngine;
using YG;

namespace Shudder.Services
{
    public class LeaderBoardsService
    {
        private MenuConfig _config;
        private readonly ITriggerOnlyEventBus _triggerOnlyEvent;
        private readonly InputService _inputService;
        private readonly UIRootView _uiRootView;

        public LeaderBoardsService(DIContainer container)
        {
            _triggerOnlyEvent = container.Resolve<ITriggerOnlyEventBus>();
            _inputService = container.Resolve<InputService>();
            _uiRootView = container.Resolve<UIRootView>();
            
        }

        public LeaderboardYG Leaderboard { get; private set; }
        
        public void Init(MenuConfig config)
        {
            _config = config;
        }

        public void CreateRewardWindow()
        {
            Debug.Log("CreateRewardWindow");
            _inputService.Disable();
            var window = Object.Instantiate(_config.UILeaderboardsView);
            window.Bind(_triggerOnlyEvent, _inputService);
            _uiRootView.AttachUI(window.gameObject);
            Leaderboard = window.leaderboardYg;
        }
    }
}