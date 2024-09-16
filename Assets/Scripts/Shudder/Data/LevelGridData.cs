using System;
using Shudder.Models;

namespace Shudder.Data
{
    [Serializable]
    public class LevelGridData
    {
        public int Level;
        public bool IsBuilt;
        public float SpaceBetweenCells;
        public float ChanceDestroy;

        public int Width;
        public int Height;
       
        public int CountCoin;
        public float ChanceCoin;

        public bool IsKey;
        
        public Cell[,] Cells;
    }
}