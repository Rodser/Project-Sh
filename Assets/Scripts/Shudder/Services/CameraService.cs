using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Shudder.Models;
using Shudder.Vews;
using UnityEngine;
using DG.Tweening;

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
            deviation.y += 1f;
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
               View.transform.position = GetCurve(startPosition, deviation, target, timeInFly);

            await UniTask.Yield();
            }
        }

        private Vector3 GetCurve(Vector3 a, Vector3 b, Vector3 c, float time)
        {
            Vector3 ab = Vector3.Lerp(a, b, time);
            Vector3 bc = Vector3.Lerp(b, c, time);

            return Vector3.Lerp(ab, bc, time);
        }

        public async Task Diving(Vector3 position)
        {
            var path = GetPath(position);
            position.y -= 2f;
            var duration = 1.5f;
            var numJumps = 1;
            var jumpPower = 1.5f;
            View.transform.DOJump(position, jumpPower, numJumps, duration);
            await UniTask.Delay(1000);
        }
        
        private Vector3[] GetPath(Vector3 position)
        {
            var point0 = position;
            point0.y++;
            var point1 = position;
            var point2 = position;
            point0.y -= 3f;
            
            return new[] {point0, point2};
        }
    }
}