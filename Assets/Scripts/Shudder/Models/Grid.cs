using Shudder.Models.Interfaces;
using Shudder.Presenters;
using UnityEngine;

namespace Shudder.Models
{
    public class Grid : IGrid
    {
        public Ground[,] Grounds { get;  set; }
        public Ground Hole { get;  set; }
        public Vector3 OffsetPosition { get ;  set ; }
        public GridPresenter Presenter { get; set; }
        public int CountPit { get; set; }
    }
}