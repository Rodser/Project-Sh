using BaCon;
using Shudder.Data;
using Shudder.Events;
using UnityEngine;

namespace Shudder.Services
{
    public class CoinService
    {
        private const int SingleCoin = 7;
        private const int DefaultCache = 100;
        
        private readonly SfxService _sfxService;
        
        private int _cache;
        private int _coinCount;
        private StorageService _storageService;


        public CoinService(DIContainer container)
        {
            _sfxService = container.Resolve<SfxService>();
            
            var readOnlyEvent = container.Resolve<IReadOnlyEventBus>();
            readOnlyEvent.TakeCoin.AddListener(TakeCoin);
        }

        public void Init(StorageService storageService)
        {
            _storageService = storageService;
        }

        public int MakeMoney(int level)
        {
            _cache += level * SingleCoin + DefaultCache;
            return _cache;
        }

        public int GetRewardedBonus()
        {
            return Random.Range(1, 10) * 50;
        }

        private void TakeCoin()
        {
            _coinCount++;
            _sfxService.TakeCoin();
            var coin = _coinCount * SingleCoin;
            _storageService.UpCoin(coin);
            _cache += coin;
        }

        public int GetRewardedNextLevelBonus()
        {
            return _cache * 2;
        }
            
        public int GetEarnedCoin()
        {
            return _cache;
        }
            
        public void Clear()
        {
            _cache = 0;
            _coinCount = 0;
        }
    }
}