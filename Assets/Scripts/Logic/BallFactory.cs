using Rodser.Config;
using Rodser.Model;
using UnityEngine;

namespace Rodser.Logic
{
    public class BallFactory
    {
        private readonly BallConfig _ballConfig;
        private readonly HexogenGridConfig _hexGridConfig;

        public BallFactory(BallConfig ballConfig, HexogenGridConfig hexGridConfig)
        {
            _ballConfig = ballConfig;
            _hexGridConfig = hexGridConfig;
        }

        internal Ball Create(Vector3 offsetPosition)
        {
            var position = GetStartPosition() + offsetPosition;

            var ball = Object.Instantiate(_ballConfig.Prefab, position, Quaternion.identity);
            ball.SetSpeed(_ballConfig.SpeedMove);
            return ball;
        }

        private Vector3 GetStartPosition()
        {
            float x = _ballConfig.StartPositionX * _hexGridConfig.SpaceBetweenCells;
            float y = _ballConfig.StartPositionY;
            float z = _ballConfig.StartPositionZ * _hexGridConfig.SpaceBetweenCells;
            
            return new Vector3(x, y, z);
        }
    }
}