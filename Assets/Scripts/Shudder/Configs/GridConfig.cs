using UnityEngine;

namespace Shudder.Configs
{
    [CreateAssetMenu(fileName = "Grid", menuName = "Game/HexogenGrid", order = 7)]
    public class GridConfig : ScriptableObject
    {
        public int Level;
        public GroundConfig GroundConfig = null;
        public float SpaceBetweenCells = 0.86f;
        public float ChanceDestroy = 0.2f;

        public int Width = 5;
        public int Height = 10;
       
        public int CountCoin = 5;
        public float ChanceCoin = 0.5f;

        public Vector2Int HolePositionForWidth;
        public Vector2Int HolePositionForHeight;

        public bool IsKey;
        public Vector2Int KeyPositionForWidth;
        public Vector2Int KeyPositionForHeight;

        public bool IsPit;
        public int PitCount;
        public float ChanceOfPit;
        public Vector2Int PitPositionForWidth;
        public Vector2Int PitPositionForHeight;

        public bool IsWall;
        public float ChanceOfWall;
        public Vector2Int WallPositionForWidth;
        public Vector2Int WallPositionForHeight;
        
        public bool IsBuilt;
    }
}
