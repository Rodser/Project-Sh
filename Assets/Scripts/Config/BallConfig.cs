using Rodser.Model;
using UnityEngine;

namespace Rodser.Config
{
    [CreateAssetMenu(fileName = "Ball", menuName = "Game/Ball", order = 9)]
    public class BallConfig : ScriptableObject
    {
        [field: SerializeField] public Ball Prefab { get; internal set; }
        [field: SerializeField] public int StartPositionX { get; internal set; }
        [field: SerializeField] public int StartPositionY { get; internal set; }
        [field: SerializeField] public int StartPositionZ { get; internal set; }
    }
}