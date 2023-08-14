using Rodser.Core;
using UnityEngine;

namespace Rodser.Config
{
    [CreateAssetMenu(fileName = "Game", menuName = "Game/Game", order = 6)]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public HexogenGridConfig MenuGridConfig { get; private set; } = null;
        [field: SerializeField] public HexogenGridConfig LevelGridConfig { get; private set; } = null;
        [field: SerializeField] public BallConfig BallConfig { get; private set; } = null;
        [field: SerializeField] public HUD HUD { get; private set; } = null;

    }
}