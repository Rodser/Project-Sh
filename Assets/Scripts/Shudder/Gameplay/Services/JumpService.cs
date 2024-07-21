using Cysharp.Threading.Tasks;
using DG.Tweening;
using Shudder.Gameplay.Configs;
using UnityEngine;

namespace Shudder.Gameplay.Services
{
    public class JumpService
    {
        private Vector3 _initialScale;

        public async UniTask Jump(JumpConfig jumpConfig, Transform viewTransform, Transform target)
        {
            _initialScale = viewTransform.localScale;
            
            await Clench(jumpConfig, viewTransform);
            //Stretch(heroConfig,viewTransform);
            await JumpTo(jumpConfig,viewTransform, target);
            //await Clench(jumpConfig,viewTransform);
        }

        private async UniTask Clench(JumpConfig jumpConfig, Transform viewTransform)
        {
            var scale = new Vector3(_initialScale.x * jumpConfig.ScaleClench.x,_initialScale.y * jumpConfig.ScaleClench.y,_initialScale.z * jumpConfig.ScaleClench.z);
            
            var tween = viewTransform.DOScale(scale, jumpConfig.DurationClench);
            await tween.AsyncWaitForCompletion();
            
            viewTransform.DOScale(_initialScale, jumpConfig.DurationClench);
        }

        private void Stretch(Transform viewTransform)
        {
            var scale = new Vector3(_initialScale.x * 0.6f,_initialScale.y * 1.7f,_initialScale.z * 0.6f);
            var duration = 0.5f;
            viewTransform.DOScale(scale, duration);
        }
        
        private async UniTask JumpTo(JumpConfig jumpConfig, Transform viewTransform, Transform target)
        {
            var tween = viewTransform.DOJump(target.position, jumpConfig.PowerJump, 1, jumpConfig.DurationJump);
            await tween.AsyncWaitForCompletion();
        }
    }
}