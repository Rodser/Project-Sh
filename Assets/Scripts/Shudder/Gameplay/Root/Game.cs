using DI;
using Shudder.Events;
using Shudder.Gameplay.Services;
using Shudder.Models;
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
        public CameraFollow CameraFollow { get; set; }
        public Grid CurrentGrid { get; private set; }

        public Game(DIContainer container)
        {
            _container = container;
            CameraFollow = _container.Resolve<CameraService>().CameraFollow;
            _container.Resolve<IReadOnlyEventBus>().ChangeHeroPosition.AddListener(OnChangeHeroPosition);
        }

        public void Run()
        {
            FlyCameraAsync();
            _container.Resolve<CameraSurveillanceService>().Follow(CameraFollow.Presenter.View);
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
            {
                for (var y = 0; y < CurrentGrid.Grounds.GetLength(1); y++)
                {
                    var ground = CurrentGrid.Grounds[x, y];

                    if(ground.GroundType == GroundType.Pit)
                        continue;
                    Object.Destroy(ground.Presenter.View.gameObject);
                    ground.Presenter.View = null;
                }
            }

            CurrentGrid.Grounds = null;
            Object.Destroy(CurrentGrid.Presenter.View.gameObject);
            CurrentGrid = null;
        }

        private void OnChangeHeroPosition(Vector3 position) => 
            _heroPosition = position;

        private async void FlyCameraAsync()
        {
            await _container.Resolve<CameraService>().MoveCameraAsync(_heroPosition);
        }
    }
}