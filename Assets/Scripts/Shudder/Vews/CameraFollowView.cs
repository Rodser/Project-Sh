using Shudder.Presenters;
using UnityEngine;

namespace Shudder.Vews
{
    public class CameraFollowView : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        
        public CameraFollowPresenter Presenter { get; private set; }
        public Camera Camera => _camera;

        public void Construct(CameraFollowPresenter presenter)
        {
            Presenter = presenter;
            Presenter.SetView(this);
        }
    }
}