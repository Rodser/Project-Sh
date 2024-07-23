using System;

namespace Core
{
    internal class CoinSystem
    {
        private Action<int> _changeCoin;
        private int _coin;

        public CoinSystem(Action<int> changeCoin)
        {
            _changeCoin = changeCoin;
            _changeCoin?.Invoke(_coin);
        }

        internal void Change()
        {
            _coin += (int)(100 * UnityEngine.Random.value); 
            _changeCoin?.Invoke(_coin);
        }
    }
}