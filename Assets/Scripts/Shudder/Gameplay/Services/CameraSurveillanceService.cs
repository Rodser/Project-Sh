using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Shudder.Gameplay.Models.Interfaces;
using Shudder.Views;

namespace Shudder.Gameplay.Services
{
    public class CameraSurveillanceService
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        
        private CameraFollowView _cameraFollowView;
        private IHero _hero;

        public void Follow(CameraFollowView cameraFollowView, IHero hero)
        {
            _cameraFollowView = cameraFollowView;
            _hero = hero;
            ChangePosition().ToCancellationToken(_cancellationTokenSource.Token);
        }
        
        public void UnFollow()
        {
            _cameraFollowView = null;
            _hero = null;
            _cancellationTokenSource.Cancel();
        }

        private async UniTask ChangePosition()
        {
            const float duration = 0.6f;
            while (_hero is not null)
            {
                var heroView = _hero.Presenter.View;
                if(heroView is null)
                    return;
                var move = _cameraFollowView.transform.DOMove(heroView.transform.position, duration);
                await move.AsyncWaitForCompletion();
            }
        }
    }
}