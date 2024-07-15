using Shudder.Presenters;

namespace Shudder.Models.Interfaces
{
    public interface IGrid
    {
        GridPresenter Presenter { get; set; }
        Ground[,] Grounds { get; set; }
        Ground Hole { get; set; }
    }
}