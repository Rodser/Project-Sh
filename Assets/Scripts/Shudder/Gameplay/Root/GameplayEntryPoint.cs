using BaCon;
using Cysharp.Threading.Tasks;
using Shudder.Configs;
using Shudder.Factories;
using Shudder.Gameplay.Configs;
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
            _container = container;

            InitializeCameraService();
            Registration();

            _game = new Game(_container, _gameConfig);
            var loadingService = _container.Resolve<LevelLoadingService>();
            loadingService.Init(_game);
            _container
                .Resolve<SettingService>().
                Init(_gameConfig.HudView.UISettingView, loadingService, _container.Resolve<CameraSurveillanceService>());

            await loadingService.LoadAsync();
        }

        private void Registration()
        {
            _container.RegisterFactory("LevelGrid",c => 
                new GridFactory(_container, _gameConfig.LevelGridConfigs)).AsSingle();
            _container.RegisterFactory(c => new BuilderGridService(_container)).AsSingle();
            _container.RegisterFactory(c => new GroundFactory(_container)).AsSingle();
            _container.RegisterFactory(c => new LightFactory()).AsSingle();      
            _container.RegisterFactory(c => new ItemFactory()).AsSingle();      
            _container.RegisterFactory(c => new JewelKeyFactory()).AsSingle();
            _container.RegisterFactory(c => new LiftService());
            _container.RegisterFactory(c => new SwapService(_container));
            _container.RegisterFactory(c => 
                    new HeroFactory(_container, _gameConfig.GetConfig<HeroConfig>())).AsSingle();
       
            _container.RegisterInstance(new CameraSurveillanceService(_container));
            _container.RegisterInstance(new HeroMoveService(_container, _gameConfig.GetConfig<HeroConfig>()));
            _container.RegisterInstance(new CheckingPossibilityOfJumpService());
            _container.RegisterInstance(new LevelLoadingService(_container, _gameConfig));
            _container.RegisterInstance(new VictoryHandlerService(_container, _gameConfig));
            _container.RegisterInstance(new IndicatorService(_container, _gameConfig));
            _container.RegisterInstance(new ActivationPortalService());

        }
        
        private void InitializeCameraService()
        {
            CameraFollowView cameraFollowView = FindFirstObjectByType<CameraFollowView>();
            
            CameraFollow cameraFollow = cameraFollowView is null 
                ? new CameraFollowFactory().Create() 
                : cameraFollowView.Presenter.CameraFollow;  
            
            _container.RegisterInstance(new CameraService(cameraFollow));
        }
    }
}