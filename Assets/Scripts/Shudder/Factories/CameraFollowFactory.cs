using Shudder.Models;
using Shudder.Presenters;
using Shudder.Views;
using UnityEngine;

namespace Shudder.Factories
{
    public class CameraFollowFactory
    {
        public CameraFollow Create(CameraFollowView prefab)
        {
            var cameraFollow = Instantiate(prefab);
            Object.DontDestroyOnLoad(cameraFollow.Presenter.View.gameObject);
            
            return cameraFollow;
        }
        
        public CameraFollow Instantiate(CameraFollowView prefab)
        {
            var cameraFollowView = Object.Instantiate(prefab);
            var cameraFollow = new CameraFollow();
            var presenter = new CameraFollowPresenter(cameraFollow);
            cameraFollowView.Construct(presenter);
            
            return cameraFollow;
        }
    }
}