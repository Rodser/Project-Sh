using Rodser.Config;
using Rodser.Logic;
using Rodser.Model;

namespace Rodser.Core
{
    public class Game
    {

        internal async void Start(HexGridConfig hexGridConfig)
        {
            GridFactory gridFactory = new GridFactory();
            HexGrid hexGrid = await gridFactory.Create(hexGridConfig);

            InputSystem input = new InputSystem();
            input.Initialize();
            MoveSystem moveSystem = new MoveSystem(input, hexGrid);
        }
    }
}