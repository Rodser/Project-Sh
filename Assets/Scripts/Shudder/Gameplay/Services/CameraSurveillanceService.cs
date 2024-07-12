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
            var end = new Vector3(position.x, _camera.transform.position.y, position.z - 2f);

            var time = 0f;
            while (time < 1f)
            {
                const float speed = 0.6f;
                time += speed * Time.deltaTime;
                _camera.transform.position = Vector3.Lerp(start, end, time);

                await UniTask.Yield();
            }
        }
    }
}