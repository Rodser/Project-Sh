using BaCon;
using Shudder.Events;
using Shudder.Gameplay.Models;
using Shudder.Gameplay.Services;
using Shudder.MainMenu.Configs;
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
        private readonly MenuConfig _menuConfig;
        private readonly Hero _hero;
        private readonly ITriggerOnlyEventBus _triggerEventBus;

        public MainMenu(DIContainer container, Grid menuGrid, MenuConfig menuConfig, Hero hero)
        {
            _container = container;
            _menuGrid = menuGrid;
            _menuConfig = menuConfig;
            _hero = hero;

            _triggerEventBus = _container.Resolve<ITriggerOnlyEventBus>();
            var readEventBus = _container.Resolve<IReadOnlyEventBus>();
            
            readEventBus.PlayGame.AddListener(OnPlayGame);
        }

        private async void OnPlayGame()
        {
            var cameraService = _container.Resolve<CameraService>();
            await _container
                .Resolve<JumpService>()
                .Jump(
                    _menuConfig.HeroConfig.JumpConfig,
                    _hero.Presenter.View.transform,
                    _menuGrid.Portal.AnchorPoint
                    );
            
            await cameraService.MoveCameraAsync(_menuGrid.Portal.AnchorPoint.position, 2f);
            
            _triggerEventBus.TriggerStartGameplayScene();
        }
    }
}