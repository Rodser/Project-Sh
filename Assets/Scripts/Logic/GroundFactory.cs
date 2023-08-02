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

        public GroundFactory(HexGridConfig hexGridConfig, Transform parent)
        {
            _spaceBetweenCells = hexGridConfig.SpaceBetweenCells;
            _prefab = hexGridConfig.Prefab;
            _parent = parent;
        }

        internal Ground Create(int x, int z)
        {
            float rowOffset = z % 2 * 0.5f;
            int raised = GetHeight();

            Vector3 positionCell = new Vector3
            {
                x = (x + rowOffset) * _spaceBetweenCells,
                y = raised,
                z = z * _spaceBetweenCells * InnerRadiusCoefficient
            };

            var ground = Object.Instantiate(_prefab, positionCell, Quaternion.identity, _parent);
            ground.Raise(raised > 0);
            return ground;
        }

        private int GetHeight()
        {
            return 0 + Random.Range(0, 2);
        }
    }
}