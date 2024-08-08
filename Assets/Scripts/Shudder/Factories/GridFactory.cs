using System;
using Cysharp.Threading.Tasks;
using DI;
using Shudder.Configs;
using Shudder.Presenters;
using Shudder.Services;
using Shudder.Vews;
using UnityEngine;
using Grid = Shudder.Models.Grid;

namespace Shudder.Factories
{
    public class GridFactory
    {
        private readonly DIContainer _container;
        private readonly GridConfig[] _hexogenGridConfigs;
        private readonly GridConfig _gridConfig;

        public GridFactory(DIContainer container, GridConfig[] hexogenGridConfigs)
        {
            _container = container;
            _hexogenGridConfigs = hexogenGridConfigs;
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

            var builderGridService = _container.Resolve<BuilderGridService>() ?? 
                                     throw new ArgumentNullException("_container.Resolve<BuilderGridService>()");
            if (isMenu)
            {
               return await builderGridService
                    .CreateGrounds(grid, _gridConfig, isMenu)
                    .EstablishPit()
                    .EstablishPortal()
                    .GetBuild();
            }
            else
            {
                return await builderGridService
                    .CreateGrounds(grid, _hexogenGridConfigs[level], isMenu)
                    .EstablishPit()
                    .EstablishPortal()
                    .GetBuild();
            }
        }
    }
}