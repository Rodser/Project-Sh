using Shudder.Events;
using Shudder.Services;
using TMPro;
using UnityEngine;

namespace Shudder.UI
{
    public class UIRewardWindowView : PopUpView
    {
       [SerializeField] private TextMeshProUGUI _coinTM;
      
       private int _coin;
       private bool _nextLevel;

       public void Bind(ITriggerOnlyEventBus eventBus, InputService inputService, bool nextLevel)
       {
           base.Bind(eventBus, inputService);
           _nextLevel = nextLevel;
       }

       public void TakeMoney()
        {
            _triggerOnlyEvent.TriggerUpdateCoin(_coin);
            
            if (_nextLevel) 
                _triggerOnlyEvent.TriggerPlayNextLevel();

            CloseWindow();
        }
        
        public void SetCoin(int value)
        {
            _coin = value;
            _coinTM.text = $"+{value}";
        }
    }
}