using Cysharp.Threading.Tasks;
using DG.Tweening;
using Shudder.Views;

namespace Shudder.Gameplay.Services
{
    public class LiftService
    {
        public async UniTask MoveAsync(GroundView groundView, float offset, bool isMenu)
        {
            if(groundView == null)
                return;

            var heightOffset = 0.4f;
            if (isMenu)
            {
                heightOffset = 0.25f;
            }
            
            var height = (int)groundView.Presenter.Ground.GroundType * heightOffset + offset;
            var target = groundView.transform.position;
            target.y = height;

            var move = groundView.transform.DOMove(target, 0.1f);
            await move.AsyncWaitForCompletion();
        }
    }
}