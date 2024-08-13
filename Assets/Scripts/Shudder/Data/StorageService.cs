using BaCon;
using Shudder.Events;
using Shudder.Services;
using YG;

namespace Shudder.Data
{
    public class StorageService
    {
        private readonly ITriggerOnlyEventBus _triggerOnlyEvent;
        private readonly CoinService _coinService;

        public StorageService(DIContainer container)
        {
            _coinService = container.Resolve<CoinService>();
            _coinService.Init(this);
            _triggerOnlyEvent = container.Resolve<ITriggerOnlyEventBus>();
            
            container.Resolve<IReadOnlyEventBus>().UpdateCoin.AddListener(UpCoin);
        }

        public PlayerProgress Progress { get; private set; }

        public void SaveProgress()
        {
            YandexGame.savesData.PlayerProgress = Progress;
            YandexGame.SaveProgress();
        }

        public void LoadProgress()
        {
            Progress = YandexGame.savesData.PlayerProgress;
        }

        public int LevelUp(int maxLevel)
        {
            var coin = _coinService.MakeMoney();
            Progress.Coin += coin;
            if(Progress.Level < maxLevel)
                Progress.Level++;
            SaveProgress();
            _triggerOnlyEvent.TriggerUpdateUI();
            return coin;
        }

        public void UpCoin(int value)
        {
            Progress.Coin += value;
            SaveProgress();
            _triggerOnlyEvent.TriggerUpdateUI();
        }
    }
}