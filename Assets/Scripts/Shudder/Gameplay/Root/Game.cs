using Core;
using UnityEngine;

namespace Shudder.Gameplay.Root
{
    public class Game
    {
        public int CurrentLevel { get; set; }
        public Camera Camera { get; set; }
        public BodyGrid Body { get; set; }

        public void DieBody()
        {
            if(Body == null)
                return;
            Object.Destroy(Body.gameObject);
            Body = null;
        }

        private void OnNotify(bool isVictory)
        {
            //if (!isVictory)
                return;

            //_coinSystem.Change();
                                    
           // if(_currentLevel + 1 < _gameConfig.LevelGridConfigs.Length)
             //   _currentLevel++;
        }
    }
}