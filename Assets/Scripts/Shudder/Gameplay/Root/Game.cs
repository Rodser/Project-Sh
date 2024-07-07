using Core;
using DI;
using Shudder.Gameplay.Characters.Views;
using Shudder.Gameplay.Services;
using UnityEngine;

namespace Shudder.Gameplay.Root
{
    public class Game
    {
        private readonly DIContainer _container;
        public int CurrentLevel { get; set; }
        public Camera Camera { get; set; }
        public BodyGrid Body { get; set; }
        public Vector3 HeroPosition { get; set; }

        public Game(DIContainer container)
        {
            _container = container;
        }

        public void Run()
        {
            FlyCameraAndStartGameplayAsync();
        }

        public void DieBody()
        {
            if(Body == null)
                return;
            Object.Destroy(Body.gameObject);
            Body = null;
        }
        
        private async void FlyCameraAndStartGameplayAsync()
        {
            var cameraService = _container.Resolve<CameraService>();
            var position = HeroPosition;
            position.y += 10;
            await cameraService.MoveCameraAsync(position);
            
            // _triggerEventBus.TriggerStartGameplayScene();
        }
        
        private void OnNotify(bool isVictory)
        {
            //if (!isVictory)
                return;

            //_coinSystem.Change();
                                    
           // if(_currentLevel + 1 < _gameConfig.LevelGridConfigs.Length)
             //   _currentLevel++;
        }
    }
}