using DI;
using Shudder.Events;
using Shudder.Gameplay.Services;
using Shudder.Services;
using UnityEngine;
using Grid = Shudder.Models.Grid;

namespace Shudder.Gameplay.Root
{
    public class Game
    {
        private readonly DIContainer _container;

        private Vector3 _heroPosition;
        
        public int CurrentLevel { get; set; }
        public Camera Camera { get; set; }
        public Grid CurrentGrid { get; private set; }

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

        public void SetCurrentGrid(Grid currentGrid)
        {
            CurrentGrid = currentGrid;
        }

        public void DestroyGrid()
        {
            if(CurrentGrid is null)
                return;
            for (var x = 0; x < CurrentGrid.Grounds.GetLength(0); x++)
            {for (var y = 0; y < CurrentGrid.Grounds.GetLength(1); y++)
            {
                var ground = CurrentGrid.Grounds[x, y];
                
                Object.Destroy(ground.Presenter.View.gameObject);
                ground.Presenter.View = null;
            }}

            CurrentGrid.Grounds = null;
            Object.Destroy(CurrentGrid.Presenter.View.gameObject);
            CurrentGrid = null;
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