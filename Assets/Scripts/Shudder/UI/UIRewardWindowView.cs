using TMPro;
using UnityEngine;

namespace Shudder.UI
{
    public class UIRewardWindowView : PopUpView
    {
       [SerializeField] private TextMeshProUGUI _coinTM;

       public void TakeMoney()
        {
            _triggerOnlyEventBus.TriggerUpdateCoin();
            CloseWindow();
        }
        
        public void SetCoin(int value)
        {
            _coinTM.text = $"+{value}";
        }
    }
}