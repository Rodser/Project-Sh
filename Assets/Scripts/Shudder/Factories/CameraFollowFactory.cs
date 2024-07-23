using Shudder.Models;
using Shudder.Presenters;
using Shudder.Vews;
using UnityEngine;

namespace Shudder.Factories
{
    public class CameraFollowFactory
    {
        public CameraFollow Create()
        {
            var prefab = Resources.Load<CameraFollowView>("Camera");
            var cameraFollowView = Object.Instantiate(prefab);
            Object.DontDestroyOnLoad(cameraFollowView.gameObject);

            var cameraFollow = new CameraFollow();
            var presenter = new CameraFollowPresenter(cameraFollow);
            cameraFollowView.Construct(presenter);
            
            return cameraFollow;
        }
    }
}