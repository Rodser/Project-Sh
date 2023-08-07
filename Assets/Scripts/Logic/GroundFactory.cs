using Rodser.Config;
using Rodser.Model;
using UnityEngine;

namespace Rodser.Logic
{
    internal class GroundFactory
    {
        private const float InnerRadiusCoefficient = 0.86f;

        private readonly float _spaceBetweenCells;
        private readonly Ground _prefab;
        private readonly Transform _parent;
        private readonly GroundConfig _groundConfig;

        public GroundFactory(HexGridConfig hexGridConfig, Transform parent)
        {
            _groundConfig = hexGridConfig.GroundConfig;
            _spaceBetweenCells = hexGridConfig.SpaceBetweenCells;
            _prefab = hexGridConfig.GroundConfig.Prefab;
            _parent = parent;
        }

        internal Ground Create(int x, int z, GroundType groundType)
        {
            float rowOffset = z % 2 * 0.5f;

            Vector3 positionCell = new Vector3
            {
                x = (x + rowOffset) * _spaceBetweenCells,
                y = 0f,
                z = z * _spaceBetweenCells * InnerRadiusCoefficient
            };

            var ground = Object.Instantiate(_prefab, positionCell, Quaternion.identity, _parent);
            ground.Set(new Vector2(x, z), _groundConfig, groundType);
            ground.Lift();
            return ground;
        }
    }
}