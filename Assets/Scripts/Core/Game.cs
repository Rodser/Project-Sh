using System;
using Logic;
using Model;
using Rodser.Config;
using Rodser.Logic;
using Rodser.Model;
using Rodser.System;
using UnityEngine;
using Object = UnityEngine.Object;

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

        public void Initialize(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
            _bodyFactory = new BodyFactory();
            _menuGridFactory = new GridFactory(gameConfig.MenuGridConfig);
            _gridFactory = new GridFactory(gameConfig.LevelGridConfig);
            _ballFactory = new BallFactory(gameConfig.BallConfig, gameConfig.LevelGridConfig);
            _lightFactory = new LightFactory();

            _camera = Camera.main;
            _input = new InputSystem();
            StartMenu();
        }

        private void LoadInterface(HUD hud)
        {
            _hud = Object.Instantiate(hud);
        }

        private async void StartMenu()
        {
            LoadInterface(_gameConfig.Hud);
            _body = _bodyFactory.Create();

            _currentGrid = await _menuGridFactory.Create(_body.transform, true);
            Object.Instantiate(_gameConfig.Title, _currentGrid.Hole.transform);
            _lightFactory.Create(_gameConfig.Light, _camera.transform, _body.transform);
            _hud.StartButton.onClick.AddListener(StartLevelAsync);
        }

        private async void StartLevelAsync()
        {
            CameraSystem cameraSystem = new CameraSystem();
            await cameraSystem.MoveCameraAsync(_currentGrid.Hole.transform.position, _camera);
            LoadLevelAsync();
        }

        private async void LoadLevelAsync()
        {
            Debug.Log("Load Level 1");
            _body.Did();
            _body =_bodyFactory.Create();
            _lightFactory.Create(_gameConfig.Light, _camera.transform, _body.transform);

            _currentGrid = await _gridFactory.Create(_body.transform);
            Ball ball = _ballFactory.Create(_currentGrid.OffsetPosition);
            
            _input.Initialize();
            MoveSystem moveSystem = new MoveSystem(_input);            
            BallSystem ballSystem = new BallSystem(ball, _currentGrid.Hole.transform.position);
        }
    }

    public class LightFactory
    {
        public Light Create(Light light, Transform cameraTransform, Transform bodyTransform)
        {
            return Object.Instantiate(light, cameraTransform.position, cameraTransform.rotation, bodyTransform);
        }
    }
}