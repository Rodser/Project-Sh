using Cysharp.Threading.Tasks;
using Shudder.Models;
using Shudder.Vews;
using UnityEngine;
using DG.Tweening;

namespace Shudder.Services
{
    public class CameraService
    {
        private Vector3 _startPosition;
        private Vector3 _startRotation;

        public CameraService(CameraFollow cameraFollow)
        {
            CameraFollow = cameraFollow;
        }

        public Camera Camera => View.Camera;
        public CameraFollow CameraFollow { get; }
        public CameraFollowView View => CameraFollow.Presenter.View;
        
        public async UniTask MoveCameraAsync(Vector3 target, float durationMove)
        {
            View.transform.DOJump(target, 1.4f, 1, durationMove);
            View.transform.DORotate(Vector3.zero, durationMove);
            await UniTask.Delay((int)(durationMove * 1000));

            target.y -= 2f;
            View.transform.DOMove(target, 0.4f, true);
            await UniTask.Delay(400);
        }
        
        public async UniTask MoveCameraAsync(Vector3 targetPosition, Vector3 targetRotation, float durationMove = 3f)
        {
            View.transform.DOMove(targetPosition, durationMove);
            View.transform.DORotate(targetRotation, durationMove);
            
            await UniTask.Delay((int)(durationMove * 1000));
        }
    }
}