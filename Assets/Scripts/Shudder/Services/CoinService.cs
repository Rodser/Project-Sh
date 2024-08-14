using BaCon;
using Shudder.Data;
using Shudder.Events;
using UnityEngine;

namespace Shudder.Services
{
    public class CoinService
    {
        private int _cache;
        private StorageService _storageService;
        private readonly SfxService _sfxService;

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
            _cache += level * 11 + 111;
            var coin = _cache;
            _cache = 0;

            return coin;
        }

        public int GetRewardedBonus()
        {
            return Random.Range(1, 10) * 50;
        }

        private void TakeCoin()
        {
            _sfxService.TakeCoin();
            _storageService.UpCoin(10);
            _cache += 77;
        }

        public int GetRewardedNextLevelBonus()
        {
            return _cache * 2;
        }
    }
}