using Cysharp.Threading.Tasks;
using Logic;
using Model;
using Rodser.Config;
using Rodser.Logic;
using Rodser.Model;
using Rodser.System;
using UnityEngine;

namespace Core
{
    public class Game
    {
        private GridFactory _menuGridFactory;
        private GridFactory _gridFactory;
        private BallFactory _ballFactory;
        private InputSystem _input;
        private HUD _hud;
        private HexogenGrid _currentGrid;
        private BodyGrid _body;
        private BodyFactory _bodyFactory;

        public void Initialize(GameConfig gameConfig)
        {
            _bodyFactory = new BodyFactory();
            _menuGridFactory = new GridFactory(gameConfig.MenuGridConfig);
            _gridFactory = new GridFactory(gameConfig.LevelGridConfig);
            _ballFactory = new BallFactory(gameConfig.BallConfig, gameConfig.LevelGridConfig);

            _input = new InputSystem();
            StartMenu(gameConfig);
        }


        private void LoadInterface(HUD hud)
        {
            _hud = Object.Instantiate(hud);
        }

        private async void StartMenu(GameConfig gameConfig)
        {
            LoadInterface(gameConfig.Hud);
            _body = _bodyFactory.Create();

            _currentGrid = await _menuGridFactory.Create(_body.transform, true);
            Object.Instantiate(gameConfig.Title, _currentGrid.Hole.transform);
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
            _body.Did();
            _body =_bodyFactory.Create();
            _currentGrid = await _gridFactory.Create(_body.transform);
            Ball ball = _ballFactory.Create(_currentGrid.OffsetPosition);
            
            _input.Initialize();
            MoveSystem moveSystem = new MoveSystem(_input);            
            BallSystem ballSystem = new BallSystem(ball, _currentGrid.Hole.transform.position);
        }

        // TODO: Выделить систему 
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