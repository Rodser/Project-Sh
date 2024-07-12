using Config;
using Core;
using Cysharp.Threading.Tasks;
using DI;
using Logic;
using Shudder.Configs;
using Shudder.Events;
using Shudder.Factories;
using Shudder.Gameplay.Factories;
using Shudder.Gameplay.Services;
using Shudder.Models;
using Shudder.Services;
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

            _game = new Game(_container);

            await _container.Resolve<LevelLoadingService>().LoadAsync(_game);
            Subscribe();

            _game.Run();
        }

        private void InitializeFactories()
        {
            _container.RegisterSingleton("LevelGrid",c => 
                new GridFactory(_container, _gameConfig.LevelGridConfigs));
            _container.RegisterSingleton(c => new BodyFactory());
            _container.RegisterSingleton(c => new GroundFactory(_container));
            _container.RegisterSingleton(c => new HeroFactory(_container, _gameConfig));
            _container.RegisterSingleton(c => new LightFactory());
            _container.RegisterSingleton(c => new SoundFactory(_gameConfig.GetConfig<SFXConfig>()));
        }

        private void InitializeServices()
        {      
            _container.RegisterSingleton(c => new CameraSurveillanceService(_container, Camera.main));
            _container.RegisterSingleton(c => new HeroMoveService(_container));
            _container.RegisterSingleton(c => new CheckingPossibilityOfJumpService());
            _container.RegisterSingleton(c => new LevelLoadingService(_container, _gameConfig));
            _container.RegisterSingleton(c => new VictoryHandlerService(_container, _gameConfig));
            _container.RegisterSingleton(c => new IndicatorService(_container, _gameConfig));
            _container.RegisterTransient(c => new LiftService());
            _container.RegisterTransient(c => new SwapService(_container));

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

        private void OnHasVictory()
        {
            _container.Resolve<VictoryHandlerService>().HasVictory(_game);
        }
    }
}