using Config;
using Core;
using Cysharp.Threading.Tasks;
using DI;
using Shudder.Configs;
using Shudder.Events;
using Shudder.Factories;
using Shudder.Gameplay.Configs;
using Shudder.Gameplay.Factories;
using Shudder.Gameplay.Services;
using Shudder.Models;
using Shudder.Services;
using Shudder.UI;
using Shudder.Vews;
using UnityEngine;

namespace Shudder.Gameplay.Root
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        [SerializeField] private GameConfig _gameConfig = null;
        
        private DIContainer _container;
        private Game _game;

        public async UniTask Initialisation(DIContainer container)
        {
            _container = new DIContainer(container);

            InitializeCameraService();
            InitializeFactories();
            InitializeServices();

            _game = new Game(_container, _gameConfig);

            await _container.Resolve<LevelLoadingService>().LoadAsync(_game);
            Subscribe();

            _game.Run();
        }

        private void InitializeFactories()
        {
            _container.RegisterSingleton("LevelGrid",c => 
                new GridFactory(_container, _gameConfig.LevelGridConfigs));
            _container.RegisterSingleton(c => new BuilderGridService(_container));
            _container.RegisterSingleton(c => new GroundFactory(_container));
            _container.RegisterSingleton(c => new LightFactory());      
            _container.RegisterSingleton(c => new ItemFactory());      
            _container.RegisterSingleton(c => new JewelKeyFactory());
            _container.RegisterSingleton(c => new HeroFactory(_container, _gameConfig.GetConfig<HeroConfig>()));
            _container.RegisterSingleton(c => new SoundFactory(_gameConfig.GetConfig<SFXConfig>()));
        }

        private void InitializeServices()
        {      
            _container.RegisterSingleton(c => new CameraSurveillanceService(_container));
            _container.RegisterSingleton(c => new HeroMoveService(_container, _gameConfig.GetConfig<HeroConfig>()));
            _container.RegisterSingleton(c => new CheckingPossibilityOfJumpService());
            _container.RegisterSingleton(c => new LevelLoadingService(_container, _gameConfig));
            _container.RegisterSingleton(c => new VictoryHandlerService(_container, _gameConfig));
            _container.RegisterSingleton(c => new IndicatorService(_container, _gameConfig));
            _container.RegisterTransient(c => new LiftService());
            _container.RegisterTransient(c => new SwapService(_container));
            _container.RegisterSingleton(c => new JumpService());
            _container.RegisterSingleton(c => new ActivationPortalService());

        }
        
        private void InitializeCameraService()
        {
            CameraFollowView cameraFollowView = FindFirstObjectByType<CameraFollowView>();
            
            CameraFollow cameraFollow = cameraFollowView is null 
                ? new CameraFollowFactory().Create() 
                : cameraFollowView.Presenter.CameraFollow;  
            
            _container.RegisterSingleton(c => new CameraService(cameraFollow));
        }

        private void Subscribe()
        {
            _container.Resolve<IReadOnlyEventBus>().HasVictory.AddListener(OnHasVictory);
        }

        private async void OnHasVictory(Transform groundAnchorPoint)
        {
            _container.Resolve<CameraSurveillanceService>().UnFollow();
            
            await _container
                .Resolve<CameraService>()
                .MoveCameraAsync(groundAnchorPoint.position, 2f);
            
            _container.Resolve<UIRootView>().ShowLoadingScreen();
            _container.Resolve<VictoryHandlerService>().HasVictory(_game);
        }
    }
}