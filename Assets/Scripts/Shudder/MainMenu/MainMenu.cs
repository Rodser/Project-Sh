using DI;
using Model;
using Shudder.Events;
using Shudder.Gameplay.Services;
using UnityEditor;
using UnityEngine;

namespace Shudder.MainMenu
{
    public class MainMenu
    {
        private readonly DIContainer _container;
        private readonly HexogenGrid _menuGrid;
        private readonly ITriggerOnlyEventBus _triggerEventBus;

        public MainMenu(DIContainer container, HexogenGrid menuGrid)
        {
            _container = container;
            _menuGrid = menuGrid;

            _triggerEventBus = _container.Resolve<ITriggerOnlyEventBus>();
            var readEventBus = _container.Resolve<IReadOnlyEventBus>();
            
            readEventBus.FlyCamera.AddListener(FlyCameraAndStartGameplayAsync);
            readEventBus.ExitGame.AddListener(ExitGame);
        }
        private async void FlyCameraAndStartGameplayAsync()
        {
            var cameraService = _container.Resolve<CameraService>();
            await cameraService.MoveCameraAsync(_menuGrid.Hole.transform.position);
            
            _triggerEventBus.TriggerStartGameplayScene();
        }
        
        private void ExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
    }
}