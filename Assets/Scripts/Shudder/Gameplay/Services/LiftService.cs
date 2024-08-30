using Cysharp.Threading.Tasks;
using DG.Tweening;
using Shudder.Views;

namespace Shudder.Gameplay.Services
{
    public class LiftService
    {
        private const float HeightOffset = 0.25f;
        
        public async UniTask MoveAsync(GroundView groundView, float offset, bool isMenu)
        {
            if(groundView == null)
                return;
            
            var height = (int)groundView.Presenter.Ground.GroundType * HeightOffset + offset;
            var target = groundView.transform.position;
            target.y = height;

            var move = groundView.transform.DOMove(target, 0.1f);
            await move.AsyncWaitForCompletion();
        }
    }
}