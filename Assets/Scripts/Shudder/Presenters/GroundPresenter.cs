using Shudder.Models.Interfaces;
using Shudder.Views;

namespace Shudder.Presenters
{
    public class GroundPresenter 
    {
        public GroundPresenter(IGround ground)
        {
            Ground = ground;
        }

        public IGround Ground { get; }
        public GroundView View { get; set; }

        public void SetView(GroundView groundView)
        {
            View = groundView;
            SetPresenter();
        }

        private void SetPresenter()
        {
            Ground.Presenter = this;
        }
    }
}