using Cysharp.Threading.Tasks;
using Rodser.Config;
using Rodser.Logic;
using Rodser.Model;
using Rodser.System;
using UnityEngine;

namespace Rodser.Core
{
    public class Game
    {
        private GridFactory _menuGridFactory;
        private GridFactory _gridFactory;
        private BallFactory _ballFactory;
        private InputSystem _input;
        private HUD _hud;
        private HexogenGrid _currentGrid;

        public void Initialize(GameConfig _gameConfig)
        {            
            LoadInterface(_gameConfig.HUD);
            _menuGridFactory = new GridFactory(_gameConfig.MenuGridConfig);
            _gridFactory = new GridFactory(_gameConfig.LevelGridConfig);
            _ballFactory = new BallFactory(_gameConfig.BallConfig, _gameConfig.LevelGridConfig);

            _input = new InputSystem();
            StartMenu();
            //Start();
        }

        private void LoadInterface(HUD hud)
        {
            _hud = Object.Instantiate(hud);
        }

        private async void StartMenu()
        {
            _currentGrid = await _menuGridFactory.Create(true);
            _hud.StartButton.onClick.AddListener(StartLevelAsync);
        }

        private async void StartLevelAsync()
        {
            await MoveCameraAsync();
            LoadLevelAsync();
        }

        private async void LoadLevelAsync()
        {
            Debug.Log("Load Level 1");
            _currentGrid = await _gridFactory.Create();
            Ball ball = _ballFactory.Create(_currentGrid.OffsetPosition);
            
            _input.Initialize();
            MoveSystem moveSystem = new MoveSystem(_input);            
            BallSystem ballSystem = new BallSystem(ball, _currentGrid.Hole.transform.position);
        }

        private async UniTask MoveCameraAsync()
        {
            Debug.Log("Move Camera");
            Camera camera = Camera.main;
            Vector3 target = _currentGrid.Hole.transform.position;

            
            var up = Vector3.Lerp(camera.transform.position, target, 0.5f);
            up.z += 3f;
            await Fly(camera.transform.position, up ,target);
        }
        private async UniTask Fly(Vector3 startPosition, Vector3 upPosition, Vector3 target)
        {
            var timeInFly = 0f;
            while (timeInFly < 1)
            {
                await UniTask.Yield();
                float speedFlying = 1f;
                timeInFly += speedFlying * Time.deltaTime;
                Camera.main.transform.position = GetCurve(startPosition, upPosition, target, timeInFly);
            }
        }

        private static Vector3 GetCurve(Vector3 point0, Vector3 point1, Vector3 point2, float time)
        {
            Vector3 point01 = Vector3.Lerp(point0, point1, time);
            Vector3 point02 = Vector3.Lerp(point1, point2, time);

            Vector3 point12 = Vector3.Lerp(point01, point02, time);

            return point12;
        }
    }
}