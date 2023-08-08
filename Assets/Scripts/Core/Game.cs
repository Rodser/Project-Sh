using Rodser.Config;
using Rodser.Logic;
using Rodser.Model;
using Rodser.System;

namespace Rodser.Core
{
    public class Game
    {
        internal async void Start(HexGridConfig hexGridConfig, BallConfig ballConfig)
        {
            GridFactory gridFactory = new GridFactory(hexGridConfig);
            HexGrid hexGrid = await gridFactory.Create();

            BallFactory ballFactory = new BallFactory(ballConfig, hexGridConfig);
            Ball ball = ballFactory.Create();

            InputSystem input = new InputSystem();
            input.Initialize();
            MoveSystem moveSystem = new MoveSystem(input);

            BallSystem ballSystem = new BallSystem(ball, hexGrid.Hole.transform.position);
        }
    }
}