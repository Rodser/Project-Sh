using BaCon;
using Shudder.Factories;
using Shudder.Gameplay.Factories;
using Shudder.Gameplay.Services;
using Shudder.MainMenu.Configs;
using Shudder.MainMenu.Factories;
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
            _container = container;
            _container.Resolve<ShopService>().Init(_menuConfiguration);
            _container.Resolve<RewardService>().Init(_menuConfiguration);
            _container.Resolve<LeaderBoardsService>().Init(_menuConfiguration);
            _container.Resolve<SfxService>().StopMusic();

            Registration();

            var menuFactory = new MainMenuFactory(_container, _menuConfiguration);
            menuFactory.Create();
        }
        
        private void Registration()
        {
            _container.RegisterInstance(new BuilderGridService(_container));
            _container.RegisterInstance(new LiftService());
            
            _container.RegisterFactory("MenuGrid",c => 
                new GridFactory(_container, _menuConfiguration.MenuGridConfig)).AsSingle();
            _container.RegisterFactory(c => new GroundFactory(_container)).AsSingle();
            _container.RegisterFactory(c => new ItemFactory()).AsSingle();   
            _container.RegisterFactory(c => new HeroFactory(_container, _menuConfiguration.HeroConfig)).AsSingle();
        }
    }
}