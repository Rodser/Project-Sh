using Config;
using Core;
using UnityEngine;

namespace Shudder.Gameplay.Root
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        [SerializeField] private GameConfig _gameConfig = null;

        public void Run()
        {
            Game game = new Game();
            game.Initialize(_gameConfig);
        }
    }
}