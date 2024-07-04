using System;
using Config;
using DI;
using Logic;
using Model;
using Shudder.Gameplay.Characters.Factoryes;
using Shudder.UI;
using UI;
using UnityEngine;

namespace Core
{
    public class Game
    {
        private InputSystem _input;
        private UserInterface _userInterface;
        private CoinSystem _coinSystem;
        private HexogenGrid _currentGrid;
        private BodyGrid _body;
        private Camera _camera;
        private GameConfig _gameConfig;
        private DIContainer _container;
        
        private int _currentLevel = 0;
        private BallMovementSystem _ballMovementSystem;
        private NotifySystem _notifySystem;

        public event Action<int, bool> ChangeHealth;
        public event Action<int> ChangeCoin;

        public void Run(DIContainer container, GameConfig gameConfig)
        {
            _container = container;
            _gameConfig = gameConfig;
        
            StartMenu();
            _input = _container.Resolve<InputSystem>();
            _input.Initialize();
        }

        private async void StartMenu()
        {
            _body = _container.Resolve<BodyFactory>().Create();

            _currentGrid = await _container.Resolve<GridFactory>("MenuGrid").Create(_body.transform, true);
            _camera = _container.Resolve<CameraSystem>().Camera;
            UnityEngine.Object.Instantiate(_gameConfig.Title, _currentGrid.Hole.transform);
            _container.Resolve<LightFactory>().Create(_gameConfig.Light, _camera.transform, _body.transform);

            _userInterface = LoadInterface();
            _userInterface.Construct(_input, this, _container.Resolve<SoundFactory>(), StartLevelAsync, OnNotify);
            _coinSystem = new CoinSystem(ChangeCoin);
        }

        private UserInterface LoadInterface()
        {
            var prefab = _gameConfig.UserInterface;
            var userInterface = UnityEngine.Object.Instantiate(prefab);
            _container.Resolve<UIRootView>().AttachSceneUI(userInterface.gameObject);
            return userInterface;
        }

        private async void LoadLevelAsync(int level)
        {
            Debug.Log($"Load Level {level}");
            _body.Did();
            _input.Clear();

            _body =_container.Resolve<BodyFactory>().Create();
            _container.Resolve<LightFactory>().Create(_gameConfig.Light, _camera.transform, _body.transform);

            _currentGrid = await _container.Resolve<GridFactory>("LevelGrid").Create(level, _body.transform);
            Ball ball = _container.Resolve<BallFactory>().Create(_currentGrid.OffsetPosition, level, _body, ChangeHealth);
            var boomSFX = _container.Resolve<SoundFactory>().Create(SFX.Boom);
            _ballMovementSystem = new BallMovementSystem(_input, ball, boomSFX, _camera);
            _notifySystem = new NotifySystem(ball, _userInterface);

            var hero = _container.Resolve<HeroFactory>().Create(_currentGrid.OffsetPosition, level, _body.gameObject);
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
            await _container.Resolve<CameraSystem>().MoveCameraAsync(_currentGrid.Hole.transform.position);
            LoadLevelAsync(_currentLevel);
        }
    }
}