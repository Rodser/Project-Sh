using Cysharp.Threading.Tasks;
using DG.Tweening;
using DI;
using Shudder.Gameplay.Configs;
using Shudder.Services;
using UnityEngine;

namespace Shudder.Gameplay.Services
{
    public class JumpService
    {
        private readonly DIContainer _container;
        private Vector3 _initialScale;

        public JumpService(DIContainer container)
        {
            _container = container;
        }

        public async UniTask Jump(JumpConfig jumpConfig, Transform viewTransform, Transform target)
        {
            _container.Resolve<SfxService>().Jump();
            _container.Resolve<AnimationHeroService>().Jump();
            _container.Resolve<RotationService>().LookRotation(viewTransform, target.position);
            _initialScale = viewTransform.localScale;
            await UniTask.Delay((int)(jumpConfig.DurationClench * 1000f));
            await JumpTo(jumpConfig,viewTransform, target);
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