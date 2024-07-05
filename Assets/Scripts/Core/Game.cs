using System;
using Config;
using Cysharp.Threading.Tasks;
using DI;
using Logic;
using Model;
using Shudder.Gameplay.Characters.Factoryes;
using Shudder.Gameplay.Services;
using Shudder.UI;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class Game
    {
        private InputService _input;
        private UserInterface _userInterface;
        private CoinSystem _coinSystem;
        private HexogenGrid _currentGrid;
        private BodyGrid _body;
        private Camera _camera;
        private GameConfig _gameConfig;
        private DIContainer _container;
        
        private int _currentLevel = 0;
        private NotifySystem _notifySystem;

        public event UnityAction OnStartLevel;
        public event Action<int, bool> ChangeHealth;
        public event Action<int> ChangeCoin;

        public void Run(DIContainer container, GameConfig gameConfig)
        {
            _container = container;
            _gameConfig = gameConfig;
        
            StartMenu();
            _input = _container.Resolve<InputService>();

            OnStartLevel += StartLevel;
        }

        private void StartLevel()
        {
            StartLevelAsync();
        }

        private async void StartMenu()
        {
            _body = _container.Resolve<BodyFactory>().Create();

            _currentGrid = await _container.Resolve<GridFactory>("MenuGrid").Create(_body.transform, true);
            _camera = _container.Resolve<CameraService>().Camera;
            UnityEngine.Object.Instantiate(_gameConfig.Title, _currentGrid.Hole.transform);
            _container.Resolve<LightFactory>().Create(_gameConfig.Light, _camera.transform, _body.transform);

            _userInterface = LoadInterface();
            _userInterface.Construct(_input, 
                this,
                _container.Resolve<SoundFactory>(),
                OnStartLevel,
                OnNotify);
            _coinSystem = new CoinSystem(ChangeCoin);
        }

        private UserInterface LoadInterface()
        {
            var prefab = _gameConfig.UserInterface;
            var userInterface = UnityEngine.Object.Instantiate(prefab);
            _container.Resolve<UIRootView>().AttachSceneUI(userInterface.gameObject);
            return userInterface;
        }

        private async UniTask LoadLevelAsync(int level)
        {
            Debug.Log($"Load Level {level}");
            _body.Did();
            _input.Clear();

            _body =_container.Resolve<BodyFactory>().Create();
            _container.Resolve<LightFactory>().Create(_gameConfig.Light, _camera.transform, _body.transform);

            _currentGrid = await _container.Resolve<GridFactory>("LevelGrid").Create(level, _body.transform);
            //var boomSFX = _container.Resolve<SoundFactory>().Create(SFX.Boom);

            var heroView = _container.Resolve<HeroFactory>().Create(_currentGrid.OffsetPosition, level, _body.gameObject);
            var moveService = _container.Resolve<HeroMoveService>();
            moveService.Subscribe(heroView);
        }

        private void OnNotify(bool isVictory)
        {
            if (!isVictory)
                return;

            _coinSystem.Change();
                                    
            if(_currentLevel + 1 < _gameConfig.LevelGridConfigs.Length)
                _currentLevel++;
        }

        private async void StartLevelAsync()
        {
            _userInterface.PlayMusic();

            var cameraService = _container.Resolve<CameraService>();
            await cameraService.MoveCameraAsync(_currentGrid.Hole.transform.position);
            
            await LoadLevelAsync(_currentLevel);
        }
    }
}