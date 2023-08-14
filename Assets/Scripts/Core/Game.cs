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
            await _menuGridFactory.Create(true);
        }

        private async void Start()
        {
            HexogenGrid hexGrid = await _gridFactory.Create();
            Ball ball = _ballFactory.Create(hexGrid.OffsetPosition);

            _input.Initialize();
            MoveSystem moveSystem = new MoveSystem(_input);            
            BallSystem ballSystem = new BallSystem(ball, hexGrid.Hole.transform.position);
        }
    }
}