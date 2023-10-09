using System;
using Config;
using Logic;
using Model;
using Rodser.Config;
using UI;
using UnityEngine;

namespace Core
{
    public class Game
    {
        private GridFactory _menuGridFactory;
        private GridFactory _gridFactory;
        private BallFactory _ballFactory;
        private BodyFactory _bodyFactory;
        private LightFactory _lightFactory;
        
        private InputSystem _input;
        private UserInterface _userInterface;
        private HexogenGrid _currentGrid;
        private BodyGrid _body;
        private Camera _camera;
        private GameConfig _gameConfig;
        
        private int _currentLevel = 0;
        private BallMovementSystem _ballMovementSystem;
        private NotifySystem _notifySystem;
        private CameraSystem _cameraSystem;
        private int _coin;

        public event Action<int, bool> ChangeHealth;
        public event Action<int> ChangeCoin;

        public void Initialize(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
        
            InitializeFactory(gameConfig);
            InitializeSystem();
            StartMenu();
        }

        private void InitializeFactory(GameConfig gameConfig)
        {
            _bodyFactory = new BodyFactory();
            _menuGridFactory = new GridFactory(gameConfig.MenuGridConfig);
            _gridFactory = new GridFactory(gameConfig.LevelGridConfigs);
            _ballFactory = new BallFactory(gameConfig.BallConfig, gameConfig.LevelGridConfigs);
            _lightFactory = new LightFactory();
        }

        private void InitializeSystem()
        {
            _cameraSystem = new CameraSystem(Camera.main);
            _camera = _cameraSystem.Camera;
            _input = new InputSystem();
            _input.Initialize();
        }

        private async void StartMenu()
        {
            LoadInterface(_gameConfig.UserInterface);
            _body = _bodyFactory.Create();

            _currentGrid = await _menuGridFactory.Create(_body.transform, true);
            UnityEngine.Object.Instantiate(_gameConfig.Title, _currentGrid.Hole.transform);
            _lightFactory.Create(_gameConfig.Light, _camera.transform, _body.transform);
            
            var musicSource = UnityEngine.Object.Instantiate(_gameConfig.Music);
            _userInterface.Construct(_input, this, musicSource, StartLevelAsync, OnNotify);
            ChangeCoin?.Invoke(_coin);
        }

        private void LoadInterface(UserInterface userInterface)
        {
            //TODO: Create interface Factory
            _userInterface = UnityEngine.Object.Instantiate(userInterface);
        }

        private async void LoadLevelAsync(int level)
        {
            Debug.Log($"Load Level {level}");
            _body.Did();
            _input.Clear();

            _body =_bodyFactory.Create();
            _lightFactory.Create(_gameConfig.Light, _camera.transform, _body.transform);

            _currentGrid = await _gridFactory.Create(level, _body.transform);
            Ball ball = _ballFactory.Create(_currentGrid.OffsetPosition, level, _body, ChangeHealth);

            _ballMovementSystem = new BallMovementSystem(_input, ball, _camera);
            _notifySystem = new NotifySystem(ball, _userInterface);
        }

        private void OnNotify(bool isVictory)
        {
            if (!isVictory)
                return;
            
            _coin += (int)(100 + UnityEngine.Random.value); // TODO: Create Coin System
            ChangeCoin?.Invoke(_coin);
            
            if(_currentLevel + 1 < _gameConfig.LevelGridConfigs.Length)
                _currentLevel++;
        }

        private async void StartLevelAsync()
        {
            await _cameraSystem.MoveCameraAsync(_currentGrid.Hole.transform.position);
            LoadLevelAsync(_currentLevel);
        }
    }
}