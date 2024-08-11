using BaCon;
using Shudder.Constants;
using Shudder.Events;
using TMPro;
using UnityEngine;
using YG;

namespace Shudder.UI
{
    public class HudView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _Level;
        [SerializeField] private TextMeshProUGUI _coin;
        [SerializeField] private TextMeshProUGUI _diamond;
        
        private ITriggerOnlyEventBus _triggerOnlyEventBus;

        public void Bind(DIContainer container)
        {
            _triggerOnlyEventBus = container.Resolve<ITriggerOnlyEventBus>();
        }
        
        public void Reward()
        {
            YandexGame.RewVideoShow(GameConstant.RewardIndex);
        }
        
        public void OpenSettings()
        {
            _triggerOnlyEventBus.TriggerOpenSettings();
        }
        
        public void SetLevel(int value)
        {
            value++;
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