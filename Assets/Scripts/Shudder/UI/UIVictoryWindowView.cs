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
        private int _coin;

        public async void PlayNextLevel()
        {
            _triggerOnlyEvent.TriggerUpdateCoin(_coin);
            await UniTask.Delay(400);
            _triggerOnlyEvent.TriggerPlayNextLevel();
            Debug.Log("Next Level");
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
            _coin = value;
            _coinTM.text = $"+{value}";
        }
    }
}