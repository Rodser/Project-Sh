using DG.Tweening;
using Shudder.Views;

namespace Shudder.Gameplay.Services
{
    public class LiftService
    {
        public async void MoveAsync(GroundView groundView, float offset, bool isMenu)
        {
            if(groundView == null)
                return;

            var heightOffset = 0.5f;
            if (isMenu)
            {
                heightOffset = 0.25f;
            }
            
            var height = (int)groundView.Presenter.Ground.GroundType * heightOffset + offset;
            var target = groundView.transform.position;
            target.y = height;

            var move = groundView.transform.DOMove(target, 0.2f);
            await move.AsyncWaitForCompletion();
        }
    }
}