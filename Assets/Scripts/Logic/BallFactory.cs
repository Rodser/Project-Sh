using Rodser.Config;
using Rodser.Model;
using UnityEngine;

namespace Rodser.Logic
{
    public class BallFactory
    {
        private readonly BallConfig _ballConfig;
        private readonly HexGridConfig _hexGridConfig;

        public BallFactory(BallConfig ballConfig, HexGridConfig hexGridConfig)
        {
            _ballConfig = ballConfig;
            _hexGridConfig = hexGridConfig;
        }

        internal Ball Create()
        {
            var ball = Object.Instantiate(_ballConfig.Prefab, GetStartPosition(), Quaternion.identity);
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