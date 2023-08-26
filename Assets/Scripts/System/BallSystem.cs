using Model;
using UnityEngine;

namespace System
{
    internal class BallSystem
    {
        private readonly Ball _ball;

        public BallSystem(Ball ball)
        {
            _ball = ball;
        }

        public void SetTarget(Vector3 position)
        {
            _ball.MoveToTargetAsync(position);
        }
    }
}