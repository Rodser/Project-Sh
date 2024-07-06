using Config;
using DI;
using Logic;
using Shudder.Gameplay.Services;
using Shudder.MainMenu.Factories;
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
            
            InitializeFactories();
            InitializeServices();

            var menuFactory = new MainMenuFactory(_container, _menuConfiguration);
            menuFactory.Create();
        }
        
        private void InitializeFactories()
        {
            _container.RegisterSingleton(c => new BodyFactory());
            _container.RegisterSingleton("MenuGrid",c => 
                new GridFactory(_menuConfiguration.MenuGridConfig));
            _container.RegisterSingleton(c =>  new LightFactory());
        }

        private void InitializeServices()
        {
            _container.RegisterSingleton(c => new CameraService(Camera.main));
            //_container.RegisterSingleton(c => new InputService());
        }
    }
}