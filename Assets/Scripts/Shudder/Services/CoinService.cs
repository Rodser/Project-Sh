using BaCon;
using UnityEngine;

namespace Shudder.Services
{
    public class CoinService
    {
        public int MakeMoney()
        {
            var coin = 222;

            // Calculate

            return coin;
        }

        public int GetRewardedBonus()
        {
            return Random.Range(1, 10) * 50;
        }
    }
}