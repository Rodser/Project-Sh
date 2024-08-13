using Cysharp.Threading.Tasks;
using Shudder.Models;
using UnityEngine;
using DG.Tweening;
using Shudder.Views;

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
            View.transform.DOJump(target, 1f, 1, durationMove);
            View.transform.DORotate(Vector3.zero, durationMove);
            await UniTask.Delay((int)(durationMove * 1000) - 100);

            target.y -= 5f;
            View.transform.DOMove(target, 0.4f);
            await UniTask.Delay(400);
        }
        
        public async UniTask MoveCameraAsync(Vector3 targetPosition, Vector3 targetRotation, float durationMove = 3f)
        {
            View.transform.DOMove(targetPosition, durationMove);
            View.transform.DORotate(targetRotation, durationMove);
            
            await UniTask.Delay((int)(durationMove * 1000));
        }

        public void Reset()
        {
            View.transform.position = Vector3.zero;
        }
    }
}