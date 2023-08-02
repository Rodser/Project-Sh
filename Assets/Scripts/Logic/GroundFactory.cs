using Rodser.Config;
using Rodser.Model;
using UnityEngine;

namespace Rodser.Logic
{
    internal class GroundFactory
    {
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
            Vector3 positionCell = new Vector3
            {
                x = (x + z % 2 * 0.5f) * _spaceBetweenCells,
                y = GetHeight(),
                z = z * _spaceBetweenCells * 0.86f
            };

            var g = Object.Instantiate(_prefab, positionCell, Quaternion.identity, _parent);
            return g;
        }

        private float GetHeight()
        {
            return 0f + UnityEngine.Random.Range(0, 2);
        }
    }
}