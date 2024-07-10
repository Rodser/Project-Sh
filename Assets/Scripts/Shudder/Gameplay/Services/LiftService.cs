using Cysharp.Threading.Tasks;
using Shudder.Gameplay.Views;
using UnityEngine;

namespace Shudder.Gameplay.Services
{
    public class LiftService
    {
        public async void MoveAsync(GroundView groundView, float offset)
        {
            var height = (int)groundView.Presenter.Ground.GroundType * 0.5f + offset;
            var target = groundView.transform.position;
            target.y = height;

            var time = 0f;
            while (time < 1)
            {
                await UniTask.Yield();
             
                time += Time.deltaTime;
                groundView.transform.position = Vector3.Lerp(groundView.transform.position, target, time);
            }
        }
    }
}