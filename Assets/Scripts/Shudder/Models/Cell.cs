using UnityEngine;

namespace Shudder.Models
{
    public class Cell
    {
        public Vector2Int ID { get; }
        public GroundType GroundType { get; set; }

        public Cell(Vector2Int id, GroundType groundType)
        {
            ID = id;
            GroundType = groundType;
        }
    }
}