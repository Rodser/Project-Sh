using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Shudder.UI
{
    public class UIVictoryWindowView : PopUpView
    {
        [SerializeField] private TextMeshProUGUI _coinTM;
        private int _coin;

        public async void PlayNextLevel()
        {
            await UniTask.Delay(400);
            _triggerOnlyEvent.TriggerUpdateCoin(_coin);
            _triggerOnlyEvent.TriggerPlayNextLevel();
            Debug.Log("Next Level");
            CloseWindow();
        }
        
        public async void GoToMenu()
        {        
            await UniTask.Delay(400);
            _triggerOnlyEvent.TriggerUpdateCoin(_coin);
            _triggerOnlyEvent.TriggerGoMenu();
            Debug.Log("Exit to menu");
            CloseWindow();
        }
        
        public void SetCoin(int value)
        {
            _coin = value;
            _coinTM.text = $"+{value}";
        }
    }
}