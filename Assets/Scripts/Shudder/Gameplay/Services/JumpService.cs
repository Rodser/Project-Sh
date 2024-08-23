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
        private readonly SfxService _sfxService;
        private readonly AnimationHeroService _animationHeroService;
        private readonly RotationService _rotationService;

        public JumpService(DIContainer container)
        {
            _sfxService = container.Resolve<SfxService>();
            _animationHeroService = container.Resolve<AnimationHeroService>();
            _rotationService = container.Resolve<RotationService>();
        }

        public async UniTask Jump(JumpConfig jumpConfig, Transform viewTransform, Transform target)
        {
            _sfxService.Jump();
            _animationHeroService.Jump();
            _rotationService.LookRotation(viewTransform, target.position);
            await UniTask.WaitForSeconds(jumpConfig.DurationClench);
            await JumpTo(jumpConfig,viewTransform, target);
        }
        
        private async UniTask JumpTo(JumpConfig jumpConfig, Transform viewTransform, Transform target)
        {
            var tween = viewTransform.DOJump(target.position, jumpConfig.PowerJump, 1, jumpConfig.DurationJump);
            await tween.AsyncWaitForCompletion();
        }
    }
}