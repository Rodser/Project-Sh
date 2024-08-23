using BaCon;
using Shudder.Constants;
using Shudder.Events;
using Shudder.Services;
using Unity.Mathematics;
using UnityEngine;
using YG;

namespace Shudder.Data
{
    public class StorageService
    {
        private readonly ITriggerOnlyEventBus _triggerOnlyEvent;

        public StorageService(DIContainer container)
        {
            var coinService = container.Resolve<CoinService>();
            coinService.Init(this);
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
            Debug.Log($"Update Coin {value}");
            Progress.Coin += value;
            Progress.Record += value;
            SaveProgress();
            _triggerOnlyEvent.TriggerUpdateUI();
        }

        public void DeductCoin(int value)
        {
            Progress.Coin = Mathf.Max(Progress.Coin - value, 0);
            SaveProgress();
            _triggerOnlyEvent.TriggerUpdateUI();
        }

        public void DeductJumpCount()
        {
            Progress.JumpCount--;
            SaveProgress();
            _triggerOnlyEvent.TriggerUpdateUI();
        }
        
        public void UpJumpCount()
        {
            Progress.JumpCount++;
            SaveProgress();
            _triggerOnlyEvent.TriggerUpdateUI();
        }
    }
}