using Rodser.Config;
using UnityEngine;

namespace Rodser.Core
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private HexGridConfig hexGridConfig = null;

        private void Awake()
        {
            Game game = new Game();
            game.Start(hexGridConfig);
        }
    }
}