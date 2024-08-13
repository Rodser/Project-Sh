using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shudder.Views
{
    public class CoinView : MonoBehaviour, IDisposable
    {
        [SerializeField] private Transform _coin;
        [SerializeField] private GameObject _coinMessage;
        
        private CancellationTokenSource _token = new();
        private bool _isRun;
        private TweenerCore<Quaternion,Vector3,QuaternionOptions> _tw;
        private float _angle = 0f;

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

        public async void TakeCoin()
        {
            _isRun = false;
            _token.Cancel();
            _coin.gameObject.SetActive(false);
            _coinMessage.SetActive(true);
            if (Camera.main is not null) 
                _coinMessage.transform.forward = Camera.main.transform.forward;
            await UniTask.Delay(1500);
            Destroy(gameObject);
        }

        public void Dispose()
        {
            _tw?.Kill();
            _token?.Cancel();
            _token?.Dispose();
        }

        private async UniTask RotateCoin()
        {
            await UniTask.Delay(Random.Range(500, 1500));
            var angle = GetAngle();
            _tw = _coin.DOLocalRotate(angle, 9f, RotateMode.FastBeyond360).SetAutoKill(false);

            while (_isRun)
            {
                await UniTask.Delay(9000);
                var oldAngle = angle;
                angle = GetAngle();
                _tw.ChangeValues(oldAngle, angle, 8f);
                _tw.Play();
            }
            _tw.Kill();
        }

        private Vector3 GetAngle()
        {
            if (_angle == 0f)
            {
                _angle = 1200f;
                return new Vector3(0f, 0f, _angle);
            }
            else
            {
                _angle = 0f;
                return Vector3.zero;
            }
        }
    }
}