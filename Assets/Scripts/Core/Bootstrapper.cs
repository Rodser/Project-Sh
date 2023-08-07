using Rodser.Config;
using UnityEngine;

namespace Rodser.Core
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private HexGridConfig _hexGridConfig = null;
        [SerializeField] private BallConfig _ballConfig = null;

        private void Awake()
        {
            Game game = new Game();
            game.Start(_hexGridConfig, _ballConfig);
        }
    }
}