using Rodser.Config;
using Rodser.Model;
using UnityEngine;

namespace Rodser.Logic
{
    internal class GroundFactory
    {
        private const float InnerRadiusCoefficient = 0.86f;

        private float _spaceBetweenCells;
        private Ground _prefab;
        private Transform _parent;
        private GroundConfig _groundConfig;

        public GroundFactory(HexGridConfig hexGridConfig, Transform parent)
        {
            _groundConfig = hexGridConfig.GroundConfig;
            _spaceBetweenCells = hexGridConfig.SpaceBetweenCells;
            _prefab = hexGridConfig.GroundConfig.Prefab;
            _parent = parent;
        }

        internal Ground Create(int x, int z)
        {
            float rowOffset = z % 2 * 0.5f;

            Vector3 positionCell = new Vector3
            {
                x = (x + rowOffset) * _spaceBetweenCells,
                y = 0f,
                z = z * _spaceBetweenCells * InnerRadiusCoefficient
            };

            var ground = Object.Instantiate(_prefab, positionCell, Quaternion.identity, _parent);
            ground.Set(new Vector2(x, z), _groundConfig);
            ground.Lift(GetHeight());
            return ground;
        }

        private int GetHeight()
        {
            return 0 + UnityEngine.Random.Range(0, 2);
        }
    }
}