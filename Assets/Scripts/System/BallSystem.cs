using Rodser.Model;
using UnityEngine;

namespace Rodser.System
{
    internal class BallSystem
    {
        private Ball _ball;

        public BallSystem(Ball ball, Vector3 holePosition)
        {
            _ball = ball;
            ball.MoveToTargetAsync(holePosition);
        }
    }
}