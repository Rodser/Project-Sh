using BaCon;
using DG.Tweening;
using Shudder.Gameplay.Models.Interfaces;
using Shudder.Vews;

namespace Shudder.Gameplay.Services
{
    public class CameraSurveillanceService
    {
        private readonly DIContainer _container;
        
        private CameraFollowView _cameraFollowView;
        private IHero _hero;

        public CameraSurveillanceService(DIContainer container)
        {
            _container = container;
        }

        public void Follow(CameraFollowView cameraFollowView, IHero hero)
        {
            _cameraFollowView = cameraFollowView;
            _hero = hero;
            ChangePosition();
        }
        
        public void UnFollow()
        {
            _cameraFollowView = null;
            _hero = null;
        }

        private async void ChangePosition()
        {
            var duration = 0.6f;
            while (_hero != null)
            {
                if(_cameraFollowView == null)
                    return; 
               
                var  move = _cameraFollowView.transform.DOMove(_hero.Presenter.View.transform.position, duration);
                await move.AsyncWaitForCompletion();
            }
        }
    }
}