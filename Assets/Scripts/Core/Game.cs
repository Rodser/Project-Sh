using Rodser.Config;
using Rodser.Logic;
using Rodser.Model;
using Rodser.System;

namespace Rodser.Core
{
    public class Game
    {

        internal async void Start(HexGridConfig hexGridConfig)
        {
            GridFactory gridFactory = new GridFactory(hexGridConfig);
            HexGrid hexGrid = await gridFactory.Create();

            InputSystem input = new InputSystem();
            input.Initialize();
            MoveSystem moveSystem = new MoveSystem(input);
        }
    }
}