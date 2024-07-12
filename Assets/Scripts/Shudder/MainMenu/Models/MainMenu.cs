using DI;
using Shudder.Events;
using Shudder.Services;
using UnityEditor;
using UnityEngine;
using Grid = Shudder.Models.Grid;

namespace Shudder.MainMenu.Models
{
    public class MainMenu
    {
        private readonly DIContainer _container;
        private readonly Grid _menuGrid;
        private readonly ITriggerOnlyEventBus _triggerEventBus;

        public MainMenu(DIContainer container, Grid menuGrid)
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
            await cameraService.MoveCameraAsync(_menuGrid.Hole.AnchorPoint.position);
            
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