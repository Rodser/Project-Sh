using Core;
using DI;
using Shudder.Events;
using Shudder.Gameplay.Services;
using UnityEngine;

namespace Shudder.Gameplay.Root
{
    public class Game
    {
        private readonly DIContainer _container;

        private Vector3 _heroPosition;
        
        public int CurrentLevel { get; set; }
        public Camera Camera { get; set; }
        public BodyGrid Body { get; set; }

        public Game(DIContainer container)
        {
            _container = container;
            Camera = _container.Resolve<CameraService>().Camera;
            _container.Resolve<IReadOnlyEventBus>().ChangeHeroPosition.AddListener(OnChangeHeroPosition);
        }

        public void Run()
        {
            FlyCameraAndStartGameplayAsync();
            _container.Resolve<CameraSurveillanceService>().Follow();
        }

        public void DieBody()
        {
            if(Body == null)
                return;
            Object.Destroy(Body.gameObject);
            Body = null;
        }

        private void OnChangeHeroPosition(Vector3 position) => 
            _heroPosition = position;

        private async void FlyCameraAndStartGameplayAsync()
        {
            var cameraService = _container.Resolve<CameraService>();
            var position = _heroPosition;
            position.y += 10;
            await cameraService.MoveCameraAsync(position);
        }
    }
}