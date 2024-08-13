using Shudder.Models.Interfaces;
using Shudder.Views;

namespace Shudder.Presenters
{
    public class GridPresenter
    {
        public GridPresenter(IGrid grid)
        {
            Grid = grid;
        }

        public IGrid Grid { get; }
        public GridView View { get; set; }

        public void SetView(GridView view)
        {
            View = view;
            SetPresenter();
        }

        private void SetPresenter()
        {
            Grid.Presenter = this;
        }
    }
}