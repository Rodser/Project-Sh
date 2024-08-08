using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shudder.UI
{
    public class HudView : MonoBehaviour
    {
        [field: SerializeField] public Button OptionButton { get; private set; }    
        [field: SerializeField] public Button LeaderboardsButton { get; private set; }    
        [field: SerializeField] public Button RewardsButton { get; private set; }    
        [field: SerializeField] public Button ShopButton { get; private set; }
        
        [SerializeField] private TextMeshProUGUI _Level;
        [SerializeField] private TextMeshProUGUI _coin;
        [SerializeField] private TextMeshProUGUI _diamond;  
        
        
        public void SetLevel(int value)
        {
            _Level.text = value.ToString();
        }
        
        public void SetCoin(int value)
        {
            _coin.text = value.ToString();
        }
        
        public void SetDiamond(int value)
        {
            _diamond.text = value.ToString();
        }
    }
}