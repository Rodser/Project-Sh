using UnityEngine;

namespace Shudder.Gameplay.Services
{
    public class AnimationHeroService
    {
        private static readonly int IsJump = Animator.StringToHash("Jump");
        
        private Animator _animator;

        public void SetAnimator(Animator animator)
        {
            _animator = animator;
        }

        public void Jump()
        {
            _animator?.SetTrigger(IsJump);
        }
    }
}