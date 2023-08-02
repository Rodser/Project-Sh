namespace Rodser.Model
{
    public class HexGrid
    {
        public Ground[] Grounds { get; internal set; }

        internal void SwapPosition()
        {
            for (int i = 0; i < Grounds.Length; i++)
            {
                Grounds[i].Swap();
            }
        }
    }
}