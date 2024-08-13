using Shudder.Models;
using Shudder.Views;

namespace Shudder.Presenters
{
    public class CameraFollowPresenter
    {
        
        public CameraFollowPresenter(CameraFollow cameraFollow)
        {
            CameraFollow = cameraFollow;
        }

        public CameraFollow CameraFollow { get; }
        public CameraFollowView View { get; set; }

        public void SetView(CameraFollowView cameraFollowView)
        {
            View = cameraFollowView;
            SetPresenter();
        }

        private void SetPresenter()
        {
            CameraFollow.Presenter = this;
        }
    }
}