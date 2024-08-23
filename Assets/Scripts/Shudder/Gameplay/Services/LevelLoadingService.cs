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
        private readonly HeroMoveService _heroMoveService;
        private readonly ItemFactory _itemFactory;
        private readonly CameraSurveillanceService _cameraSurveillanceService;
        private readonly SfxService _sfxService;
        private readonly UIRootView _uiRootView;
        private readonly GridFactory _gridFactory;
        private readonly HeroFactory _heroFactory;
        private readonly JewelKeyFactory _jewelKeyFactory;
        private readonly ActivationPortalService _activationPortalService;
        
        private Game _game;
        private StorageService _storage;
        private IReadOnlyEventBus _readOnlyEvent;
        private readonly IndicatorService _indicatorService;
        private readonly SuperJumpService _superJumpService;

        public LevelLoadingService(DIContainer container, GameConfig gameConfig)
        {
            _container = container;
            _gameConfig = gameConfig;
            _storage = container.Resolve<StorageService>();
            _victoryService = container.Resolve<VictoryHandlerService>();
            _readOnlyEvent = container.Resolve<IReadOnlyEventBus>();
            _gridFactory = container.Resolve<GridFactory>("LevelGrid");
            _jewelKeyFactory = container.Resolve<JewelKeyFactory>();
            _itemFactory = container.Resolve<ItemFactory>();
            _activationPortalService = container.Resolve<ActivationPortalService>();
            _heroFactory = container.Resolve<HeroFactory>();
            _cameraSurveillanceService = container.Resolve<CameraSurveillanceService>();
            _sfxService = container.Resolve<SfxService>();
            _uiRootView = container.Resolve<UIRootView>();
            _heroMoveService = container.Resolve<HeroMoveService>();
            _indicatorService = _container.Resolve<IndicatorService>();
            _superJumpService = _container.Resolve<SuperJumpService>();
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
            CreateCoins(currentGrid, level);
            CreateItems(currentGrid);
            CreateMusic();
            CreateHud();
            
            var hero = CreateHero(currentGrid);
            CreateActivatePortal(level, hero, currentGrid);
            _game.Hero = hero;
            _heroMoveService.Subscribe(hero);
            _superJumpService.Init(currentGrid.Grounds, hero);
            
            Debug.Log($"Load Level progress {_storage.Progress.Level}");
            _readOnlyEvent.HasVictory.AddListener(OnHasVictory);
            _game.Run();
        }
        
        private void OnHasVictory(Transform groundAnchorPoint)
        {
            _cameraSurveillanceService.UnFollow();
            
            var level = _storage.LevelUp(_gameConfig.LevelGridConfigs.Length - 1);
            _victoryService.OpenVictoryWindow(_game.SceneActiveChecked, groundAnchorPoint, level);
        }

        private void CreateHud()
        {
            var prefab = _gameConfig.HudView;
            var hudView = Object.Instantiate(prefab);
            hudView.Bind(_container);
            _game.HUD = hudView;
            _game.UpdateHud();
            _uiRootView.ChangeSceneUI(hudView.gameObject);
        }

        private void CreateMusic() => 
            _sfxService.CreateMusic(_gameConfig.GetConfig<SFXConfig>());

        private void CreateItems(Grid currentGrid) => 
            _itemFactory.Create(_gameConfig.Items, currentGrid, 0.7f);

        private void CreateActivatePortal(int level, Hero hero, Grid currentGrid)
        {
            if (!_gameConfig.LevelGridConfigs[level].IsKey)
                return;
            
            hero.ActivateTriggerKew(_gameConfig.LevelGridConfigs[level]);
            _activationPortalService.Construct(currentGrid, _gameConfig.LevelGridConfigs[level].IsKey);
            _jewelKeyFactory.Create(_gameConfig.JewelKeyView, _gameConfig.LevelGridConfigs[level], currentGrid);
        }

        private Hero CreateHero(Grid currentGrid) => 
            _heroFactory.Create(currentGrid.Grounds, _indicatorService);

        private void CreateCoins(Grid currentGrid, int level) => 
            _itemFactory.Create(
                _gameConfig.Coin,
                currentGrid,
                _gameConfig.LevelGridConfigs[level].CountCoin,
                _gameConfig.LevelGridConfigs[level].ChanceCoin);

        private async UniTask<Grid> CreateGrid(int level) => 
            await _gridFactory.Create(level);
    }
}