using Shudder.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shudder.UI
{
    public class UIMenuView : MonoBehaviour
    {
        private ITriggerOnlyEventBus _eventBus;
        
        [field: SerializeField] public Button PlayButton { get; private set; }    
        [field: SerializeField] public Button OptionButton { get; private set; }    
        [field: SerializeField] public Button LeaderboardsButton { get; private set; }    
        [field: SerializeField] public Button RewardsButton { get; private set; }    
        [field: SerializeField] public Button ShopButton { get; private set; }
        
        [SerializeField] private Image _LevelProgress;    

        [SerializeField] private TextMeshProUGUI _coin;
        [SerializeField] private TextMeshProUGUI _diamond;    
        [SerializeField] private TextMeshProUGUI _level;    

        public void Bind(ITriggerOnlyEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void StartGame()
        {
            _eventBus.TriggerFlyCamera();
            gameObject.SetActive(false);
        }
        
        public void SetCoin(int value)
        {
            _coin.text = value.ToString();
        }
        
        public void SetDiamond(int value)
        {
            _diamond.text = value.ToString();
        }

        public void SetLevel(int level, float levelProgress)
        {
            _level.text = level.ToString();
            _LevelProgress.fillAmount = levelProgress;
        }
    }
}