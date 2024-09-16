using Shudder.Presenters;
using UnityEngine;

namespace Shudder.Views
{
    public class GridView : MonoBehaviour
    {
        public GridPresenter Presenter { get; set; }

        public void Construct(GridPresenter presenter)
        {
            Presenter = presenter;
            Presenter.SetView(this);
        }
    }
}