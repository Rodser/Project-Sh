using BaCon;
using DG.Tweening;
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
        private int _currentDiamond;
        private int _currentCoin;

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
            if(_currentCoin == value)
                return;
            _currentCoin = value;
            _coin.text = value.ToString();
            _coin?.transform.DOPunchScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f).SetLink(gameObject);
        }
        
        public void SetDiamond(int value)
        {
            if(_currentDiamond == value)
                return;
            _currentDiamond = value;
            _diamond.text = value.ToString();
            _diamond?.transform.DOPunchScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
        }
    }
}