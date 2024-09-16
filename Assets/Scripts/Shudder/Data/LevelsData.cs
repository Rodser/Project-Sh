using System;
using System.Collections.Generic;

namespace Shudder.Data
{
    [Serializable]
    public class LevelsData
    {
        public readonly Dictionary<int, LevelGridData> Levels = new();

        public void Add(int level, LevelGridData levelGridData)
        {
            Levels.Add(level, levelGridData);
        }
        
        public LevelGridData Get(int level)
        {
            return Levels[level];
        }
    }
}