using Cysharp.Threading.Tasks;
using DG.Tweening;
using Shudder.Services;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shudder.Views
{
    public class CoinView : MonoBehaviour
    {
        [SerializeField] private Transform _coin;
        [SerializeField] private GameObject _coinMessage;
        [SerializeField] private TextMeshPro _coinMessageTMP;
        
        private CoinService _coinService;

        private void OnEnable()
        {
            _coinMessage.SetActive(false);
           RotateCoinAsync();
        }

        public void Construct(CoinService storageService)
        {
            _coinService = storageService;
        }

        public void TakeCoin()
        {
            _coin?.gameObject.SetActive(false);
            _coinMessage.SetActive(true);
            _coinMessage.gameObject.transform.SetParent(gameObject.transform.parent);
            _coinMessageTMP.text = $"+{_coinService.CurrentCoinValue}";
            if (Camera.main is not null) 
                _coinMessage.transform.forward = Camera.main.transform.forward;
            
            Destroy( _coinMessage.gameObject, 2f);
            Destroy(gameObject);
        }

        private async void RotateCoinAsync()
        {
            await UniTask.Delay(Random.Range(1000, 2000));
            var duration = Random.Range(5, 10);
            
            _coin?
                .DOLocalRotate(GetAngle(duration), duration, RotateMode.FastBeyond360)
                .SetLink(_coin.gameObject)
                .SetLink(gameObject)
                .SetLoops(-1, LoopType.Yoyo); 
        }

        private Vector3 GetAngle(int duration)
        {
            var angle = Random.Range(500, duration * 100);
            return new Vector3(0f, 0f, angle);
        }
    }
}