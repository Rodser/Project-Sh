using Cysharp.Threading.Tasks;
using Shudder.Constants;
using TMPro;
using UnityEngine;
using YG;

namespace Shudder.UI
{
    public class UIVictoryWindowView : PopUpView
    {
        [SerializeField] private TextMeshProUGUI _coinTM;

        public void PlayNextLevel()
        {
            Debug.Log("Next Level");
            _triggerOnlyEvent.TriggerPlayNextLevel();
            CloseWindow();
        }
        
        public async void RewardToNext()
        {        
            await UniTask.Delay(400);
            Debug.Log("Reward To Next");
            CloseWindow();
            YandexGame.RewVideoShow(GameConstant.RewardIndexNextLevel);
        }
        
        public void SetCoin(int value)
        {
            _coinTM.text = $"+{value}";
        }
    }
}