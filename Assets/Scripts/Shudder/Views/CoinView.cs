using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shudder.Views
{
    public class CoinView : MonoBehaviour
    {
        [SerializeField] private Transform _coin;
        private CancellationTokenSource _token = new();
        private bool _isRun;

        private void OnEnable()
        {
            _isRun = true;
            RotateCoin().ToCancellationToken(_token.Token);
        }

        private void OnDisable()
        {
            _isRun = false;
            _token.Cancel();
        }

        private async UniTask RotateCoin()
        {
            await UniTask.Delay(Random.Range(100, 1000));
            var tw =_coin.DOLocalRotate(new Vector3(0, 0, 1200), 4f, RotateMode.FastBeyond360).SetAutoKill(false);
            await tw.AsyncWaitForCompletion();
            
            while (_isRun)
            {
                tw.Play();
                await tw.AsyncWaitForCompletion();

                await UniTask.Delay(3000);
            }
        }
    }
}