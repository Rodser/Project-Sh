using DI;
using Shudder.Gameplay.Configs;
using Shudder.Gameplay.Models;
using Shudder.Gameplay.Services;
using Shudder.Gameplay.Views;
using Shudder.Models;
using Shudder.Models.Interfaces;
using Shudder.Presenters;
using UnityEngine;

namespace Shudder.Gameplay.Factories
{
    internal class GroundFactory
    {
        private const float InnerRadiusCoefficient = 0.86f;

        private float _spaceBetweenCells;
        private readonly DIContainer _container;
        private Transform _parent;
        private GroundConfig _groundConfig;

        public GroundFactory(DIContainer container)
        {
            _container = container;
        }

        internal Ground Create(HexogenGridConfig hexGridConfig, Transform parent, int x, int z, Vector3 offsetPosition, GroundType groundType, bool isMenu)
        {
            _groundConfig = hexGridConfig.GroundConfig;
            _spaceBetweenCells = hexGridConfig.SpaceBetweenCells;
            _parent = parent;
            
            var rowOffset = z % 2 * 0.5f;
            var positionCell = GetPositionCell(x, z, offsetPosition, rowOffset);

            var groundView = groundType switch
            {
                GroundType.Pit => GroundInstantiate(_groundConfig.PrefabPit, positionCell),
                GroundType.Hole => GroundInstantiate(_groundConfig.PrefabHole, positionCell),
                GroundType.Wall => GroundInstantiate(_groundConfig.PrefabWall, positionCell),
                _ => GroundInstantiate(_groundConfig.Prefab, positionCell)
            };
            var groundId = new Vector2(x, z);

            IGround ground = new Ground(groundId, groundType, offsetPosition);
            var presenter = new GroundPresenter(ground);
            groundView.Construct(presenter);
            
            if (!isMenu)            
                _container.Resolve<LiftService>().MoveAsync(groundView, offsetPosition.y);

            return (Ground)ground;
        }

        private GroundView GroundInstantiate(GroundView prefab, Vector3 positionCell)
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