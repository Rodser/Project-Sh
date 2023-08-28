using System;
using Logic;
using Model;
using Rodser.Config;
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
        private HUD _hud;
        private HexogenGrid _currentGrid;
        private BodyGrid _body;
        private Camera _camera;
        private GameConfig _gameConfig;
        
        private int _currentLevel = 0;
        private BallMovementSystem _ballMovementSystem;
        private NotifySystem _notifySystem;
        private CameraSystem _cameraSystem;

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
            LoadInterface(_gameConfig.Hud);
            _body = _bodyFactory.Create();

            _currentGrid = await _menuGridFactory.Create(_body.transform, true);
            UnityEngine.Object.Instantiate(_gameConfig.Title, _currentGrid.Hole.transform);
            _lightFactory.Create(_gameConfig.Light, _camera.transform, _body.transform);

            _hud.Set(_input, StartLevelAsync, OnNotify);
        }

        private void LoadInterface(HUD hud)
        {
            //TODO: Create interface Factory
            _hud = UnityEngine.Object.Instantiate(hud);
        }

        private async void LoadLevelAsync(int level)
        {
            Debug.Log($"Load Level {level}");
            _body.Did();
            _input.Clear();

            _body =_bodyFactory.Create();
            _lightFactory.Create(_gameConfig.Light, _camera.transform, _body.transform);

            _currentGrid = await _gridFactory.Create(level, _body.transform);
            Ball ball = _ballFactory.Create(_currentGrid.OffsetPosition, level, _body);

            _ballMovementSystem = new BallMovementSystem(_input, ball, _camera);
            _notifySystem = new NotifySystem(ball, _hud);
        }

        private void OnNotify(bool isVictory)
        {
            if (!isVictory)
                return;
            
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