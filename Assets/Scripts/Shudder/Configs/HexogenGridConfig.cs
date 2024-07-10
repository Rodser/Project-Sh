using UnityEngine;

namespace Shudder.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "HexogenGrid", menuName = "Game/HexogenGrid", order = 7)]
    public class HexogenGridConfig : ScriptableObject
    {
        [field: SerializeField] public GroundConfig GroundConfig { get; private set; } = null;
        [field: SerializeField] public float SpaceBetweenCells { get; private set; } = 0.86f;
        [field: SerializeField, Range(5, 20)] public int CaneraOffset { get; private set; }

        [field: Space(10), Header("Size grid")]
        [field: SerializeField, Range(4, 10)] public int Width { get; private set; } = 5;
        [field: SerializeField, Range(4, 20)] public int Height { get; private set; } = 10;
       
        [field: Space(10), Header("Hole")]
        [field: SerializeField, Range(0, 10)] public int MinHolePositionForX { get; private set; }
        [field: SerializeField, Range(1, 10)] public int MaxHolePositionForX { get; private set; }
        [field: SerializeField, Range(0, 20)] public int MinHolePositionForY { get; private set; }
        [field: SerializeField, Range(1, 20)] public int MaxHolePositionForY { get; private set; }

        [field: Space(10), Header("Pit")]
        [field: SerializeField] public int PitCount { get; private set; }
        [field: SerializeField, Range(0, 1)] public float ChanceOfPit { get; private set; }
        [field: SerializeField, Range(0, 10)] public int MinPitPositionForX { get; private set; }
        [field: SerializeField, Range(1, 10)] public int MaxPitPositionForX { get; private set; }
        [field: SerializeField, Range(0, 20)] public int MinPitPositionForY { get; private set; }
        [field: SerializeField, Range(1, 20)] public int MaxPitPositionForY { get; private set; }
    }
}
