using BaCon;
using Config;
using Cysharp.Threading.Tasks;
using Shudder.Configs;
using Shudder.Data;
using Shudder.Events;
using Shudder.Factories;
using Shudder.Gameplay.Factories;
using Shudder.Gameplay.Models;
using Shudder.Gameplay.Root;
using Shudder.Services;
using Shudder.UI;
using UnityEngine;
using Grid = Shudder.Models.Grid;

namespace Shudder.Gameplay.Services
{
    public class LevelLoadingService
    {
        private readonly DIContainer _container;
        private readonly GameConfig _gameConfig;
        private readonly VictoryHandlerService _victoryService;
        
        private Game _game;
        private StorageService _storage;
        private IReadOnlyEventBus _readOnlyEvent;

        public LevelLoadingService(DIContainer container, GameConfig gameConfig)
        {
            _container = container;
            _gameConfig = gameConfig;
            _storage = container.Resolve<StorageService>();
            _victoryService = container.Resolve<VictoryHandlerService>();
            _readOnlyEvent = container.Resolve<IReadOnlyEventBus>();
        }

        public void Init(Game game)
        {
            _game = game;
            _storage.LoadProgress();
        }

        public async UniTask DestroyLevelAsync()
        {
            _readOnlyEvent.HasVictory.RemoveListener(OnHasVictory);
            await _game.DestroyGrid();
        }

        public async UniTask LoadAsync()
        {
            var level = _storage.Progress.Level;
            var currentGrid = await CreateGrid(level);
            _game.SetCurrentGrid(currentGrid);
            CreateLights(currentGrid);
            CreateItems(currentGrid);
            CreateMusic(currentGrid);
            CreateHud();
            
            var hero = CreateHero(currentGrid);
            CreateActivatePortal(level, hero, currentGrid);
            hero.EnableIndicators();
            _game.Hero = hero;
            var moveService = _container.Resolve<HeroMoveService>();
            moveService.Subscribe(hero);
            
            Debug.Log($"Load Level progress {_storage.Progress.Level}");
            _readOnlyEvent.HasVictory.AddListener(OnHasVictory);
            _game.Run();
        }
        
        private void OnHasVictory(Transform groundAnchorPoint)
        {
            _container.Resolve<CameraSurveillanceService>().UnFollow();
            
            var coin = _storage.LevelUp(_gameConfig.LevelGridConfigs.Length - 1);
            _victoryService.OpenVictoryWindow(coin, groundAnchorPoint);
        }

        private void CreateHud()
        {
            var prefab = _gameConfig.HudView;
            var hudView = Object.Instantiate(prefab);
            hudView.Bind(_container);
            _game.HUD = hudView;
            _game.UpdateHud();
            _container.Resolve<UIRootView>().ChangeSceneUI(hudView.gameObject);
        }

        private void CreateMusic(Grid currentGrid)
        {
           var service = _container
                .Resolve<SfxService>();
           service.CreateMusic(_gameConfig.GetConfig<SFXConfig>(), currentGrid.Presenter.View.transform);
           service.StartMusic();
        }

        private void CreateItems(Grid currentGrid)
        {
            _container.Resolve<ItemFactory>().Create(_gameConfig.Items, currentGrid, 0.7f);
        }

        private void CreateActivatePortal(int level, Hero hero, Grid currentGrid)
        {
            if (!_gameConfig.LevelGridConfigs[level].IsKey)
                return;
            
            hero.ActivateTriggerKew(_gameConfig.LevelGridConfigs[level]);
            _container.Resolve<ActivationPortalService>().Construct(currentGrid, _gameConfig.LevelGridConfigs[level].IsKey);
                
            _container
                .Resolve<JewelKeyFactory>()
                .Create(_gameConfig.JewelKeyView, _gameConfig.LevelGridConfigs[level], currentGrid);
        }

        private Hero CreateHero(Grid currentGrid)
        {
            var hero = _container
                .Resolve<HeroFactory>()
                .Create(currentGrid.Grounds);
            return hero;
        }

        private void CreateLights(Grid currentGrid)
        {
            _container
                .Resolve<LightFactory>()
                .Create(_gameConfig.Lights, currentGrid, 0.2f);
        }

        private async UniTask<Grid> CreateGrid(int level)
        {
            var currentGrid = await _container
                .Resolve<GridFactory>("LevelGrid")
                .Create(level);
            return currentGrid;
        }
    }
}