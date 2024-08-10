using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Shudder.UI
{
    public class UIVictoryWindowView : PopUpView
    {
        [SerializeField] private TextMeshProUGUI _coin;

        public async void PlayNextLevel()
        {
            await UniTask.Delay(400);
            _triggerOnlyEventBus.TriggerPlayNextLevel();
            Debug.Log("Next Level");
            CloseWindow();
        }
        
        public async void GoToMenu()
        {        
            await UniTask.Delay(400);
            _triggerOnlyEventBus.TriggerGoMenu();
            Debug.Log("Exit to menu");
            CloseWindow();
        }
        
        public void SetCoin(int value)
        {
            _coin.text = $"+{value}";
        }
    }
}