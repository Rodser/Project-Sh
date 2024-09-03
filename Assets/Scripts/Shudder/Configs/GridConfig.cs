using EditorAttributes;
using UnityEngine;
using Utils;

namespace Shudder.Configs
{
    [CreateAssetMenu(fileName = "Grid", menuName = "Game/HexogenGrid", order = 7)]
    public class GridConfig : ScriptableObject
    {
        [field: SerializeField] public GroundConfig GroundConfig { get; private set; } = null;
        [field: SerializeField] public float SpaceBetweenCells { get; private set; } = 0.86f;

        [field: Space(7), Header("Size grid")]
        [field: SerializeField, Range(3, 20)] public int Width { get; private set; } = 5;
        [field: SerializeField, Range(3, 30)] public int Height { get; private set; } = 10;
       
        [field: Space(7), Header("Coins")]
        [field: SerializeField, Range(1, 50)] public int CountCoin { get; private set; } = 5;
        [field: SerializeField, Range(0, 1)] public float ChanceCoin { get; private set; } = 0.5f;
       
        [field: Space(7), Header("Portal")]
        [field: SerializeField, MinMaxSlider(1, 20)] public Vector2Int HolePositionForWidth { get; private set; }
        [field: SerializeField, MinMaxSlider(1, 30)] public Vector2Int HolePositionForHeight { get; private set; }

        [field: Space(7), Header("Key")]
        [field: SerializeField] public bool IsKey { get; private set; }
        [field: SerializeField, ShowField(nameof(IsKey)), MinMaxSlider(1, 20)] public Vector2Int KeyPositionForWidth { get; private set; }
        [field: SerializeField, ShowField(nameof(IsKey)), MinMaxSlider(1, 30)] public Vector2Int KeyPositionForHeight { get; private set; }

        [field: Space(7), Header("Pit")]
        [field: SerializeField] public bool IsPit { get; private set; }
        [field: SerializeField, ShowField(nameof(IsPit)), Clamp(0, 50)] public int PitCount { get; private set; }
        [field: SerializeField, ShowField(nameof(IsPit)), Range(0, 1)] public float ChanceOfPit { get; private set; }
        [field: SerializeField, ShowField(nameof(IsPit)), MinMaxSlider(0, 20)] public Vector2Int PitPositionForWidth { get; private set; }
        [field: SerializeField, ShowField(nameof(IsPit)), MinMaxSlider(0, 30)] public Vector2Int PitPositionForHeight { get; private set; }
        
        [field: Space(7), Header("Well")]
        [field: SerializeField] public bool IsWell { get; private set; }
        [Line(250f, 129f, 120f)]
        [field: SerializeField, ShowField(nameof(IsWell)), Range(0, 1)] public float ChanceOfWall { get; private set; }
        [field: SerializeField, ShowField(nameof(IsWell)), MinMaxSlider(0, 20)] public Vector2Int WallPositionForWidth { get; private set; }
        [field: SerializeField, ShowField(nameof(IsWell)), MinMaxSlider(0, 30)] public Vector2Int WallPositionForHeight { get; private set; }
        
        [field: Space(14)]
        [field: SerializeField] public bool IsBuilder { get; private set; }
        [Button(nameof(IsBuilder), ConditionResult.ShowHide)]
        public void BuildGrid()
        {
            new BuildingTest().BuildGrid(this);
        }
    }
}
