using Rodser.Config;
using Rodser.Logic;
using Rodser.Model;
using Rodser.System;

namespace Rodser.Core
{
    public class Game
    {
        private GridFactory _gridFactory;
        private BallFactory _ballFactory;
        private InputSystem _input;

        public void Initialize(GameConfig _gameConfig)
        {            
            _gridFactory = new GridFactory(_gameConfig.HexGridConfig);
            _ballFactory = new BallFactory(_gameConfig.BallConfig, _gameConfig.HexGridConfig);

            _input = new InputSystem();
            Start();
        }

        private async void Start()
        {
            HexGrid hexGrid = await _gridFactory.Create();
            Ball ball = _ballFactory.Create();

            _input.Initialize();
            MoveSystem moveSystem = new MoveSystem(_input);            
            BallSystem ballSystem = new BallSystem(ball, hexGrid.Hole.transform.position);
        }
    }
}