using BaCon;
using Cysharp.Threading.Tasks;
using Shudder.Configs;
using Shudder.Constants;
using Shudder.Gameplay.Services;
using Shudder.Models;
using Shudder.Models.Interfaces;
using Shudder.Presenters;
using Shudder.Vews;
using UnityEngine;

namespace Shudder.Factories
{
    internal class GroundFactory
    {
        private float _spaceBetweenCells;
        private readonly DIContainer _container;
        private Transform _parent;
        private GroundConfig _groundConfig;

        public GroundFactory(DIContainer container)
        {
            _container = container;
        }

        public async UniTask<Ground> Create(GridConfig hexGridConfig, Transform parent, int x, int z, Vector3 offsetPosition, GroundType groundType, bool isMenu)
        {
            _groundConfig = hexGridConfig.GroundConfig;
            _spaceBetweenCells = hexGridConfig.SpaceBetweenCells;
            _parent = parent;
            
            var rowOffset = z % 2 * 0.5f;
            var positionCell = GetPositionCell(x, z, offsetPosition, rowOffset);

            var groundView = groundType switch
            {
                GroundType.Pit => GroundInstantiate(_groundConfig.PrefabPit, positionCell),
                GroundType.Portal => GroundInstantiate(_groundConfig.PrefabHole, positionCell),
                GroundType.Wall => GroundInstantiate(_groundConfig.PrefabWall, positionCell),
                _ => GroundInstantiate(_groundConfig.ChoiceGroundPrefab(), positionCell)
            };
            var groundId = new Vector2(x, z);

            IGround ground = new Ground(groundId, groundType, offsetPosition);
            var presenter = new GroundPresenter(ground);
            groundView.Construct(presenter);
            
            _container.Resolve<LiftService>().MoveAsync(groundView, offsetPosition.y, isMenu);

            await UniTask.Yield();
            return (Ground)ground;
        }

        private GroundView GroundInstantiate(GroundView prefab, Vector3 positionCell)
        {
            var rotate = new Vector3(0, Random.Range(0, 6) * 60, 0);
            return Object.Instantiate(prefab, positionCell, Quaternion.Euler(rotate), _parent);
        }

        private Vector3 GetPositionCell(int x, int z, Vector3 offsetPosition, float rowOffset)
        {
            Vector3 positionCell = new Vector3
            {
                x = (x + rowOffset) * _spaceBetweenCells,
                y = 0f,
                z = z * _spaceBetweenCells * GameConstant.InnerRadiusCoefficient
            };
            positionCell += offsetPosition;
            return positionCell;
        }
    }
}