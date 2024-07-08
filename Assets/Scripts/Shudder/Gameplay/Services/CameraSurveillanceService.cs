using Cysharp.Threading.Tasks;
using DI;
using Shudder.Events;
using UnityEngine;

namespace Shudder.Gameplay.Services
{
    public class CameraSurveillanceService
    {
        private readonly DIContainer _container;
        private readonly Camera _camera;

        public CameraSurveillanceService(DIContainer container, Camera camera)
        {
            _container = container;
            _camera = camera;
        }

        public void Follow()
        {
            _container.Resolve<IReadOnlyEventBus>().ChangeHeroPosition.AddListener(OnChangePosition);
        }

        private async void OnChangePosition(Vector3 position)
        {
            var start = _camera.transform.position;
            position.y =  _camera.transform.position.y;

            var time = 0f;
            while (time < 1f)
            {
                const float speed = 0.7f;
                time += speed * Time.deltaTime;
                _camera.transform.position = Vector3.Lerp(start, position, time);

                await UniTask.Yield();
            }
        }
    }
}