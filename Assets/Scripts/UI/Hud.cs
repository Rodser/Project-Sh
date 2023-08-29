using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class Hud : MonoBehaviour
    {
        [field: SerializeField] public Button PauseButton { get; private set; }    
        [field: SerializeField] public TextMeshProUGUI Coin  { get; private set; }

        public void Subscribe(UnityAction goMenu, Action<int> changeCoin)
        {
            PauseButton.onClick.AddListener(goMenu);
            changeCoin += ChangeCoin;
        }

        private void ChangeCoin(int coin)
        {
            Coin.text = coin.ToString();
        }
    }
}