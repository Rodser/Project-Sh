using UnityEngine;

namespace Rodser.Config
{
    [CreateAssetMenu(fileName = "Game", menuName = "Game/Game", order = 6)]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public HexGridConfig HexGridConfig { get; private set; } = null;
        [field: SerializeField] public BallConfig BallConfig { get; private set; } = null;



    }
}