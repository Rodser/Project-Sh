using Cysharp.Threading.Tasks;
using DI;
using Shudder.Events;
using Shudder.Vews;
using UnityEngine;

namespace Shudder.Gameplay.Services
{
    public class CameraSurveillanceService
    {
        private readonly DIContainer _container;
        
        private CameraFollowView _cameraFollowView;

        public CameraSurveillanceService(DIContainer container)
        {
            _container = container;
        }

        public void Follow(CameraFollowView cameraFollowView)
        {
            _cameraFollowView = cameraFollowView;
            _container.Resolve<IReadOnlyEventBus>().ChangeHeroPosition.AddListener(OnChangePosition);
        }
        
        public void UnFollow()
        {
            _container.Resolve<IReadOnlyEventBus>().ChangeHeroPosition.RemoveListener(OnChangePosition);
        }

        private async void OnChangePosition(Vector3 position)
        {
            if(_cameraFollowView == null)
                return;
            
            var start = _cameraFollowView.transform.position;
            var end = new Vector3(position.x, _cameraFollowView.transform.position.y, position.z);

            var speed = 0.6f;
            var time = 0f;
            while (time < 1f)
            {
                if(_cameraFollowView == null)
                    return;
                time += speed * Time.deltaTime;
                _cameraFollowView.transform.position = Vector3.Lerp(start, end, time);

                await UniTask.Yield();
            }
        }
    }
}