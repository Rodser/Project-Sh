using Core;
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

        public void Subscribe(UnityAction goMenu, Game game)
        {
            PauseButton.onClick.AddListener(goMenu);
            game.ChangeCoin += ChangeCoin;
        }

        private void ChangeCoin(int coin)
        {
            Coin.text = coin.ToString();
        }
    }
}