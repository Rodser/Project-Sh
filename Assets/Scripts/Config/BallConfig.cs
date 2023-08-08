using Rodser.Model;
using UnityEngine;

namespace Rodser.Config
{
    [CreateAssetMenu(fileName = "Ball", menuName = "Game/Ball", order = 9)]
    public class BallConfig : ScriptableObject
    {
        [field: SerializeField] public Ball Prefab { get; private set; }
        [field: SerializeField] public int StartPositionX { get; private set; }
        [field: SerializeField] public int StartPositionY { get; private set; }
        [field: SerializeField] public int StartPositionZ { get; private set; }
        [field: SerializeField] public float SpeedMove { get; private set; }
    }
}