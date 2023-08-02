using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;

namespace Rodser.Model
{
    public class Ground : MonoBehaviour
    {
        private int _height = 0;

        public bool Raised { get; private set; }

        internal void Swap()
        {
            Lift((_height + 1) % 2);
        }

        public void Lift(int height)
        {
            _height = height;
            Raise(height > 0);
            MoveAsync();
        }

        private async void MoveAsync()
        {
            var origin = transform.position;
            var target = origin;
            target.y = _height;

            var time = 0f;
            while (time < 1)
            {
                await UniTask.Yield();
                time += 1f * Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, target, time);
            }
        }

        private void Raise(bool raised)
        {
            Raised = raised;
        }
    }
}