using Cysharp.Threading.Tasks;
using UnityEngine;

namespace System
{
    public class CameraSystem
    {
        public async UniTask MoveCameraAsync(Vector3 target, Camera camera)
        {
            Debug.Log("Move Camera");
            
            var up = Vector3.Lerp(camera.transform.position, target, 0.4f);
            up.z += 2f;
            await Fly(up ,target, camera);
        }
        private async UniTask Fly(Vector3 upPosition, Vector3 target, Camera camera)
        {
            Vector3 startPosition = camera.transform.position; 
            var timeInFly = 0f;
            while (timeInFly < 1)
            {
                await UniTask.Yield();
                float speedFlying = 1f;
                timeInFly += speedFlying * Time.deltaTime;
                camera.transform.position = GetCurve(startPosition, upPosition, target, timeInFly);
            }
        }

        private static Vector3 GetCurve(Vector3 point0, Vector3 point1, Vector3 point2, float time)
        {
            Vector3 point01 = Vector3.Lerp(point0, point1, time);
            Vector3 point02 = Vector3.Lerp(point1, point2, time);

            Vector3 point12 = Vector3.Lerp(point01, point02, time);

            return point12;
        }
    }
}