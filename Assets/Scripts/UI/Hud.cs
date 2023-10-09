using Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class Hud : MonoBehaviour
    {
        [field: SerializeField] public RectTransform healthTransform { get; private set; }    
        [field: SerializeField] public Image healthImage { get; private set; }    
        [field: SerializeField] public Button PauseButton { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Coin  { get; private set; }

        public void Subscribe(UnityAction goMenu, Game game)
        {
            PauseButton.onClick.AddListener(goMenu);
            game.ChangeCoin += ChangeCoin;
            game.ChangeHealth += ChangeHealth;
        }

        private void ChangeCoin(int coin)
        {
            Coin.text = coin.ToString();
        }

        private void ChangeHealth(int health, bool leave = false)
        {
            Debug.Log($"ChangeHealth - {health}");

            if(leave)
                healthTransform.GetChild(health).gameObject.SetActive(false);
            else
            {
                for (int i = 0; i < health; i++)
                {
                    healthTransform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
    }
}