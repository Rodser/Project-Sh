using System;
using Shudder.Constants;

namespace Shudder.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public int Level;
        public int Coin;
        public int Record;
        public int Diamond;
        public int MaxLevel;

        public PlayerProgress()
        {
            Level = InitialProgress.Level;
            MaxLevel = InitialProgress.MaxLevel;
            Coin = InitialProgress.Coin;
            Diamond = InitialProgress.Diamond;
        }

        public float GetLevelProgress()
        {
            return (Level + 1) * MaxLevel * 0.0001f;
        }
    }
}