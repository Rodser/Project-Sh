using DG.Tweening;
using Shudder.Vews;

namespace Shudder.Gameplay.Services
{
    public class LiftService
    {
        public async void MoveAsync(GroundView groundView, float offset)
        {
            if(groundView == null)
                return;
            
            var height = (int)groundView.Presenter.Ground.GroundType * 0.5f + offset;
            var target = groundView.transform.position;
            target.y = height;

            var move = groundView.transform.DOMove(target, 0.2f);
            await move.AsyncWaitForCompletion();
        }
    }
}