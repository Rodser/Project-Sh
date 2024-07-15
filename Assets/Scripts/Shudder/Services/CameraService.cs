using Cysharp.Threading.Tasks;
using Shudder.Models;
using Shudder.Vews;
using UnityEngine;

namespace Shudder.Services
{
    public class CameraService
    {
        public CameraService(CameraFollow cameraFollow)
        {
            CameraFollow = cameraFollow;
        }

        public Camera Camera => View.Camera;
        public CameraFollow CameraFollow { get; }
        public CameraFollowView View => CameraFollow.Presenter.View;

        public async UniTask MoveCameraAsync(Vector3 target)
        {
            var deviation = Vector3.Lerp(View.transform.position, target, 0.4f);
            deviation.z += 2f;
            await Fly(deviation ,target, View.transform);
        }
        
        private async UniTask Fly(Vector3 deviation, Vector3 target, Transform transform)
        {
            Vector3 startPosition = transform.position; 
            var timeInFly = 0f;
            while (timeInFly < 1f)
            {
                float speedFlying = 0.7f;
                timeInFly += speedFlying * Time.deltaTime;
                transform.position = GetCurve(startPosition, deviation, target, timeInFly);

            await UniTask.Yield();
            }
        }

        private Vector3 GetCurve(Vector3 a, Vector3 b, Vector3 c, float time)
        {
            Vector3 ab = Vector3.Lerp(a, b, time);
            Vector3 bc = Vector3.Lerp(b, c, time);

            return Vector3.Lerp(ab, bc, time);
        }
    }
}