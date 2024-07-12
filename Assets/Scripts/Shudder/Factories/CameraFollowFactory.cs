using Shudder.Models;
using Shudder.Presenters;
using Shudder.Vews;
using UnityEngine;

namespace Shudder.Factories
{
    public class CameraFactory
    {
        public CameraFollow Create()
        {
            var prefab = Resources.Load<CameraFollowView>("Camera");
            var cameraVew = Object.Instantiate(prefab);
            Object.DontDestroyOnLoad(cameraVew.gameObject);

            var camera = new CameraFollow();
            var presentor = new CameraFollowPresenter(camera);
            presentor.SetView(cameraVew);
            
            return camera;
        }
    }
}