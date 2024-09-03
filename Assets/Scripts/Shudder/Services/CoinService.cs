using BaCon;
using Shudder.Configs;
using Shudder.Data;
using Shudder.Events;
using UnityEngine;

namespace Shudder.Services
{
    public class CoinService
    {
        private readonly SfxService _sfxService;
        
        private int _cache;
        private int _coinCount;
        private StorageService _storageService;
        private PriceInfo _price;


        public CoinService(DIContainer container)
        {
            _sfxService = container.Resolve<SfxService>();
            
            var readOnlyEvent = container.Resolve<IReadOnlyEventBus>();
            readOnlyEvent.TakeCoin.AddListener(TakeCoin);
        }

        public void Init(StorageService storageService)
        {
            _storageService = storageService;
            _price = ShopService.LoadPrice();
        }

        public int MakeMoney(int level)
        {
            var levelCache = (int)(level * _price.SingleCoin * 0.5f);
            _cache += levelCache + _price.DefaultCache;
            return _cache;
        }

        public int GetRewardedBonus()
        {
            return _price.RewardBonus;
        }

        private void TakeCoin()
        {
            _coinCount++;
            _sfxService.TakeCoin();
            var coin = _coinCount * _price.SingleCoin;
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