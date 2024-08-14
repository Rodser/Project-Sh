using BaCon;
using Shudder.Constants;
using Shudder.Events;
using Shudder.Services;
using YG;

namespace Shudder.Data
{
    public class StorageService
    {
        private readonly ITriggerOnlyEventBus _triggerOnlyEvent;
        private readonly CoinService _coinService;
        private readonly LeaderBoardsService _leaderBoardsService;

        public StorageService(DIContainer container)
        {
            _coinService = container.Resolve<CoinService>();
            _coinService.Init(this);
            _leaderBoardsService = container.Resolve<LeaderBoardsService>();
            _triggerOnlyEvent = container.Resolve<ITriggerOnlyEventBus>();
            
            container.Resolve<IReadOnlyEventBus>().UpdateCoin.AddListener(UpCoin);
        }

        public PlayerProgress Progress { get; private set; }

        public void SaveProgress()
        {
            YandexGame.NewLeaderboardScores(GameConstant.NameLB, Progress.Record);
            YandexGame.savesData.PlayerProgress = Progress;
            YandexGame.SaveProgress();
        }

        public void LoadProgress()
        {
            Progress = YandexGame.savesData.PlayerProgress;
        }

        public int LevelUp(int maxLevel)
        {
            if(Progress.Level < maxLevel)
                Progress.Level++;
            SaveProgress();
            _triggerOnlyEvent.TriggerUpdateUI();
            return Progress.Level;
        }

        public void UpCoin(int value)
        {
            Progress.Coin += value;
            Progress.Record += value;
            SaveProgress();
            _triggerOnlyEvent.TriggerUpdateUI();
        }
    }
}