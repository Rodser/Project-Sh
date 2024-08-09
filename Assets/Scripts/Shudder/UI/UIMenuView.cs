using Cysharp.Threading.Tasks;
using DG.Tweening;
using Shudder.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shudder.UI
{
    public class UIMenuView : MonoBehaviour
    {
        private ITriggerOnlyEventBus _triggerOnlyEvent;
        
        [field: SerializeField] public Button PlayButton { get; private set; }    
        [field: SerializeField] public Button OptionButton { get; private set; }    
        [field: SerializeField] public Button LeaderboardsButton { get; private set; }    
        [field: SerializeField] public Button RewardsButton { get; private set; }    
        [field: SerializeField] public Button ShopButton { get; private set; }
        
        [SerializeField] private Image _LevelProgress;    

        [SerializeField] private TextMeshProUGUI _coin;
        [SerializeField] private TextMeshProUGUI _diamond;    
        [SerializeField] private TextMeshProUGUI _level;    

        public async void Bind(ITriggerOnlyEventBus eventBus)
        {
            _triggerOnlyEvent = eventBus;
            await ShowWindow();
        }

        public async void StartGame()
        {
            await CloseWindow();
            _triggerOnlyEvent.TriggerFlyCamera();
        }
        
        public void OpenSettings()
        {
            _triggerOnlyEvent.TriggerOpenSettings();
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

        private async UniTask ShowWindow()
        {
            transform.localScale = Vector3.zero;
            var tween = transform.DOScale(Vector3.one, 0.2f);
            await tween.AsyncWaitForCompletion();
        }

        private async UniTask CloseWindow()
        {
            var tween = transform.DOScale(Vector3.zero, 0.2f);
            await tween.AsyncWaitForCompletion();
        }
    }
}