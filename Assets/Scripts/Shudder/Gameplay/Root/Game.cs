using DI;
using Shudder.Gameplay.Models;
using Shudder.Gameplay.Models.Interfaces;
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

        public int CurrentLevel { get; set; }
        public CameraFollow CameraFollow { get; set; }
        public Grid CurrentGrid { get; private set; }
        public IHero Hero { get; set; }

        public Game(DIContainer container)
        {
            _container = container;
            CameraFollow = _container.Resolve<CameraService>().CameraFollow;
        }

        public async void Run()
        {
            await _container.Resolve<CameraService>().MoveCameraAsync(Hero.Presenter.View.transform.position);
            _container.Resolve<CameraSurveillanceService>().Follow(CameraFollow.Presenter.View, Hero);
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
    }
}