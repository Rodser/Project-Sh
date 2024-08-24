using Cysharp.Threading.Tasks;
using DG.Tweening;
using Shudder.Constants;
using Shudder.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Shudder.UI
{
    public class UIMenuView : MonoBehaviour
    {
        private ITriggerOnlyEventBus _triggerOnlyEvent;
        
        [SerializeField] private Image _LevelProgress;    
        [SerializeField] private TextMeshProUGUI _coinTMPro;
        [SerializeField] private TextMeshProUGUI _diamondTMPro;    
        [SerializeField] private TextMeshProUGUI _levelTMPro;
        
        private int _currentCoin;
        private int _currentDiamond;

        public async void Bind(ITriggerOnlyEventBus eventBus)
        {
            _triggerOnlyEvent = eventBus;
            await ShowWindow();
        }

        public async void StartGame()
        {
            await CloseWindow();
            _triggerOnlyEvent.TriggerPlayGame();
        }
        
        public void OpenSettings()
        {
            _triggerOnlyEvent.TriggerOpenSettings();
        }
        
        public void OpenLeaderboards()
        {
            _triggerOnlyEvent.TriggerOpenLeaderboards();
        }
        
        public void OpenReward()
        {
            YandexGame.RewVideoShow(GameConstant.RewardIndex);
        }
        
        public void OpenShop()
        {
            _triggerOnlyEvent.TriggerOpenShop();
        }
        
        public void SetCoin(int value)
        {
            _coinTMPro?.transform.localScale.Set(1f,1f,1f);

            if(_currentCoin == value)
                return;
            _currentCoin = value;
            _coinTMPro.text = value.ToString();
            _coinTMPro?.transform.DOPunchScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f).SetLink(_coinTMPro.gameObject);

        }
        
        public void SetDiamond(int value)
        {
            _diamondTMPro?.transform.localScale.Set(1f,1f,1f);

            if(_currentDiamond == value)
                return;
            _currentDiamond = value;
            _diamondTMPro.text = value.ToString();
            _diamondTMPro?.transform.DOPunchScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f).SetLink(_diamondTMPro.gameObject);
        }
        
        public void SetProgressBar(float value)
        {
            _LevelProgress.fillAmount = value;
        }
        
        public void SetLevel(int level)
        {
            level++;
            _levelTMPro.text = level.ToString();
        }

        private async UniTask ShowWindow()
        {
            transform.localScale = Vector3.zero;
            var tween = transform.DOScale(Vector3.one, 0.2f);
            await tween.AsyncWaitForCompletion();
        }

        private async UniTask CloseWindow()
        {
            var tween = transform.DOScale(new Vector3(2,2,2), 0.2f);
            await tween.AsyncWaitForCompletion();
        }
    }
}