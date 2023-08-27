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

        public void Initialize(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
            _bodyFactory = new BodyFactory();
            _menuGridFactory = new GridFactory(gameConfig.MenuGridConfig);
            _gridFactory = new GridFactory(gameConfig.LevelGridConfigs);
            _ballFactory = new BallFactory(gameConfig.BallConfig, gameConfig.LevelGridConfigs);
            _lightFactory = new LightFactory();

            _camera = Camera.main;
            _input = new InputSystem();
            StartMenu();
        }

        private void LoadInterface(HUD hud)
        {
            //TODO: Create interface Factory
            _hud = UnityEngine.Object.Instantiate(hud);
        }

        private async void StartMenu()
        {
            LoadInterface(_gameConfig.Hud);
            _body = _bodyFactory.Create();

            _currentGrid = await _menuGridFactory.Create(_body.transform, true);
            UnityEngine.Object.Instantiate(_gameConfig.Title, _currentGrid.Hole.transform);
            _lightFactory.Create(_gameConfig.Light, _camera.transform, _body.transform);
            _hud.StartButton.onClick.AddListener(StartLevelAsync);
            _hud.NextButton.onClick.AddListener(StartLevelAsync);
            _hud.NotifyEvent += OnNotify;
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
            //TODO: Fix camera system
            CameraSystem cameraSystem = new CameraSystem();
            await cameraSystem.MoveCameraAsync(_currentGrid.Hole.transform.position, _camera);
            LoadLevelAsync(_currentLevel);
        }

        private async void LoadLevelAsync(int level)
        {
            Debug.Log($"Load Level {level}");
            _body.Did();
            _body =_bodyFactory.Create();
            _lightFactory.Create(_gameConfig.Light, _camera.transform, _body.transform);

            _currentGrid = await _gridFactory.Create(level, _body.transform);
            Ball ball = _ballFactory.Create(_currentGrid.OffsetPosition, level);
            
            _input.Initialize();
            BallMovementSystem ballMovementSystem = new BallMovementSystem(_input, ball, _camera);
            NotifySystem notifySystem = new NotifySystem(ball, _hud);
        }
    }
}