using TMPro;
using UnityEngine;

namespace Shudder.UI
{
    public class UIRewardWindowView : PopUpView
    {
       [SerializeField] private TextMeshProUGUI _coinTM;
       private int _coin;

       public void TakeMoney()
        {
            _triggerOnlyEvent.TriggerUpdateCoin(_coin);
            CloseWindow();
        }
        
        public void SetCoin(int value)
        {
            _coin = value;
            _coinTM.text = $"+{value}";
        }
    }
}