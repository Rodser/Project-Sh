using BaCon;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        
        private async UniTask JumpTo(JumpConfig jumpConfig, Transform viewTransform, Transform target)
        {
            var tween = viewTransform.DOJump(target.position, jumpConfig.PowerJump, 1, jumpConfig.DurationJump);
            await tween.AsyncWaitForCompletion();
        }
    }
}