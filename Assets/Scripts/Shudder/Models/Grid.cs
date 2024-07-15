using Shudder.Models.Interfaces;
using Shudder.Presenters;

namespace Shudder.Models
{
    public class Grid : IGrid
    {
        public Ground[,] Grounds { get;  set; }
        public Ground Hole { get;  set; }
        public GridPresenter Presenter { get; set; }
        public int CountPit { get; set; }
    }
}