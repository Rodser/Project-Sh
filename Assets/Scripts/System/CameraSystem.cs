using Cysharp.Threading.Tasks;
using UnityEngine;

namespace System
{
    public class CameraSystem
    {
        public Camera Camera { get; }

        public CameraSystem(Camera camera)
        {
            Camera = camera;
        }

        public async UniTask MoveCameraAsync(Vector3 target)
        {
            Debug.Log("Move Camera");
            
            var deviation = Vector3.Lerp(Camera.transform.position, target, 0.4f);
            deviation.z += 2f;
            await Fly(deviation ,target, Camera.transform);
        }
        private async UniTask Fly(Vector3 deviation, Vector3 target, Transform transform)
        {
            Vector3 startPosition = transform.position; 
            var timeInFly = 0f;
            while (timeInFly < 1f)
            {
                await UniTask.Yield();
                float speedFlying = 0.7f;
                timeInFly += speedFlying * Time.deltaTime;
                transform.position = GetCurve(startPosition, deviation, target, timeInFly);
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