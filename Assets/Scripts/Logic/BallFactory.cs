using Core;
using Model;
using Rodser.Config;
using UnityEngine;

namespace Logic
{
    public class BallFactory
    {
        private readonly BallConfig _ballConfig;
        private readonly HexogenGridConfig[] _hexGridConfigs;

        public BallFactory(BallConfig ballConfig, HexogenGridConfig[] hexGridConfigs)
        {
            _ballConfig = ballConfig;
            _hexGridConfigs = hexGridConfigs;
        }

        internal Ball Create(Vector3 offsetPosition, int level, BodyGrid bodyGrid)
        {
            var position = GetStartPosition(level) + offsetPosition;

            var ball = Object.Instantiate(_ballConfig.Prefab, position, Quaternion.identity, bodyGrid.transform);
            ball.SetSpeed(_ballConfig.SpeedMove);
            return ball;
        }

        private Vector3 GetStartPosition(int level)
        {
            var hexGridConfig = _hexGridConfigs[level];
            float x = _ballConfig.StartPositionX * hexGridConfig.SpaceBetweenCells;
            float y = _ballConfig.StartPositionY;
            float z = _ballConfig.StartPositionZ * hexGridConfig.SpaceBetweenCells;
            
            return new Vector3(x, y, z);
        }
    }
}