﻿using BaCon;
using Cysharp.Threading.Tasks;
using Shudder.Configs;
using Shudder.Presenters;
using Shudder.Services;
using Shudder.Views;
using UnityEngine;
using Grid = Shudder.Models.Grid;

namespace Shudder.Factories
{
    public class GridFactory
    {
        private readonly DIContainer _container;
        private readonly GridConfig[] _gridConfigs;
        private readonly GridConfig _gridConfig;

        public GridFactory(DIContainer container, GridConfig[] gridConfigs)
        {
            _container = container;
            _gridConfigs = gridConfigs;
        }

        public GridFactory(DIContainer container, GridConfig gridConfig)
        {
            _container = container;
            _gridConfig = gridConfig;
        }

        public async UniTask<Grid> Create(int level = -1, bool isMenu = false)
        {
            var grid = new Grid();
            var gridView = new GameObject("Grid").AddComponent<GridView>();
            var presenter = new GridPresenter(grid);
            presenter.SetView(gridView);

            var builderGridService = _container.Resolve<BuilderGridService>();
            builderGridService.Construct(
                _container.Resolve<CameraService>(), 
                _container.Resolve<GroundFactory>());
            if (isMenu)
            {
               return await builderGridService
                    .CreateGrounds(grid, _gridConfig, true)
                    .EstablishPit()
                    .EstablishPortal()
                    .GetBuildAsync();
            }
            else
            {
                return await builderGridService
                    .CreateGrounds(grid, _gridConfigs[level], false)
                    .EstablishWall()
                    .EstablishPit()
                    .EstablishPortal()
                    .GetBuildAsync();
            }
        }
    }
}