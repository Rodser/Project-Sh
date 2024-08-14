using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shudder.Views
{
    public class CoinView : MonoBehaviour
    {
        [SerializeField] private Transform _coin;
        [SerializeField] private GameObject _coinMessage;
        
        private readonly CancellationTokenSource _token = new();
        
        private float _angle = 0f;
        private Sequence _sequence;
        private TweenerCore<Quaternion,Vector3,QuaternionOptions> _tw;

        private void OnEnable()
        {
           RotateCoinAsync().ToCancellationToken(_token.Token);
        }

        private void OnDisable()
        {
            _token.Cancel();
        }

        public void TakeCoin()
        {
            _coin?.gameObject.SetActive(false);
            _coinMessage.SetActive(true);
            if (Camera.main is not null) 
                _coinMessage.transform.forward = Camera.main.transform.forward;
            Destroy(gameObject, 2f);
        }
        
        private async UniTask RotateCoinAsync()
        {
            await UniTask.Delay(Random.Range(1000, 2000));
            var duration = Random.Range(5, 10);
            
            _coin
                .DOLocalRotate(GetAngle(duration), duration, RotateMode.FastBeyond360)
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