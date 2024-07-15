using DI;
using Shudder.Factories;
using Shudder.MainMenu.Configs;
using Shudder.MainMenu.Factories;
using Shudder.Models;
using Shudder.Services;
using UnityEngine;

namespace Shudder.MainMenu.Root
{
    public class MainMenuEntryPoint : MonoBehaviour
    {
        [SerializeField] private MenuConfig _menuConfiguration = null;
        
        private DIContainer _container;

        public void Initialisation(DIContainer container)
        {
            _container = new DIContainer(container);

            InitializeCameraService();
            InitializeFactories();

            var menuFactory = new MainMenuFactory(_container, _menuConfiguration);
            menuFactory.Create();
        }
        
        private void InitializeFactories()
        {
            _container.RegisterSingleton("MenuGrid",c => 
                new GridFactory(_container, _menuConfiguration.MenuGridConfig));
            _container.RegisterSingleton(c => new BuilderGridService(_container));
            _container.RegisterSingleton(c => new GroundFactory(_container));
            _container.RegisterSingleton(c => new LightFactory());
        }

        private void InitializeCameraService()
        {
            CameraFollow cameraFollow = new CameraFollowFactory().Create();
            _container.RegisterSingleton(c => new CameraService(cameraFollow));
        }
    }
}