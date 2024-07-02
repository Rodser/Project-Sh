using Config;
using Core;
using Rodser.Config;
using UnityEngine;

namespace Core
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private GameConfig _gameConfig = null;

        private void Awake()
        {
            Game game = new Game();
            game.Initialize(_gameConfig);
        }
    }
}