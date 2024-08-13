using BaCon;
using Shudder.Events;
using UnityEngine;

namespace Shudder.Services
{
    public class CoinService
    {
        private int _cache;

        public CoinService(DIContainer container)
        {
            var readOnlyEvent = container.Resolve<IReadOnlyEventBus>();
 
            readOnlyEvent.TakeCoin.AddListener(AddCoinToCache);
        }

        private void AddCoinToCache()
        {
            _cache += 77;
        }

        public int MakeMoney()
        {
            var coin = _cache;
            _cache = 0;

            return coin;
        }

        public int GetRewardedBonus()
        {
            return Random.Range(1, 10) * 50;
        }
    }
}