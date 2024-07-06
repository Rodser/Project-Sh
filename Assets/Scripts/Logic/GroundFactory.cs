using Config;
using Model;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Logic
{
    internal class GroundFactory
    {
        private const float InnerRadiusCoefficient = 0.86f;

        private readonly float _spaceBetweenCells;
        private readonly Transform _parent;
        private readonly GroundConfig _groundConfig;

        public GroundFactory(HexogenGridConfig hexGridConfig, Transform parent)
        {
            _groundConfig = hexGridConfig.GroundConfig;
            _spaceBetweenCells = hexGridConfig.SpaceBetweenCells;
            _parent = parent;
        }

        internal Ground Create(int x, int z, Vector3 offsetPosition, GroundType groundType, bool isMenu)
        {
            float rowOffset = z % 2 * 0.5f;

            Ground ground = null;
            var positionCell = GetPositionCell(x, z, offsetPosition, rowOffset);
            var groundId = new Vector2(x, z);

            ground = groundType switch
            {
                GroundType.Pit => GroundInstantiate(_groundConfig.PrefabPit, positionCell),
                GroundType.Hole => GroundInstantiate(_groundConfig.PrefabHole, positionCell),
                GroundType.Wall => GroundInstantiate(_groundConfig.PrefabWall, positionCell),
                _ => GroundInstantiate(_groundConfig.Prefab, positionCell)
            };

            ground.Set(groundId, groundType);

            if (!isMenu)            
                ground.Lift(offsetPosition.y);

            return ground;
        }

        private Ground GroundInstantiate(Ground prefab, Vector3 positionCell)
        {
            return Object.Instantiate(prefab, positionCell, Quaternion.identity, _parent);
        }

        private Vector3 GetPositionCell(int x, int z, Vector3 offsetPosition, float rowOffset)
        {
            Vector3 positionCell = new Vector3
            {
                x = (x + rowOffset) * _spaceBetweenCells,
                y = 0f,
                z = z * _spaceBetweenCells * InnerRadiusCoefficient
            };
            positionCell += offsetPosition;
            return positionCell;
        }
    }
}