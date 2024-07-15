﻿using System;
using Cysharp.Threading.Tasks;
using DI;
using Shudder.Configs;
using Shudder.Presenters;
using Shudder.Services;
using Shudder.Vews;
using Unity.VisualScripting;
using UnityEngine;
using Grid = Shudder.Models.Grid;

namespace Shudder.Factories
{
    public class GridFactory
    {
        private readonly DIContainer _container;
        private readonly HexogenGridConfig[] _hexogenGridConfigs;
        private readonly HexogenGridConfig _hexogenGridConfig;

        public GridFactory(DIContainer container, HexogenGridConfig[] hexogenGridConfigs)
        {
            _container = container;
            _hexogenGridConfigs = hexogenGridConfigs;
        }

        public GridFactory(DIContainer container, HexogenGridConfig hexogenGridConfig)
        {
            _container = container;
            _hexogenGridConfig = hexogenGridConfig;
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
                builderGridService = await builderGridService.CreateGrounds(grid, _hexogenGridConfig, isMenu);
                builderGridService = await builderGridService.EstablishPit();
                builderGridService = await builderGridService.EstablishHole();
            }
            else
            {
                builderGridService = await builderGridService.CreateGrounds(grid, _hexogenGridConfigs[level], isMenu);
                builderGridService = await builderGridService.EstablishPit();
                builderGridService = await builderGridService.EstablishHole();
                builderGridService = await builderGridService.SetNeighbors();
            }

            return await builderGridService.GetBuild();
        }

    }
}