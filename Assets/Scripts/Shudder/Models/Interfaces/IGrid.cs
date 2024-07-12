using Shudder.Presenters;
using UnityEngine;

namespace Shudder.Models.Interfaces
{
    public interface IGrid
    {
        GridPresenter Presenter { get; set; }
        Ground[,] Grounds { get; set; }
        Ground Hole { get; set; }
        Vector3 OffsetPosition { get ; set ; }
    }
}