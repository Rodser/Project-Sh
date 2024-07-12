using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DI;
using Shudder.Configs;
using Shudder.Models;
using Shudder.Presenters;
using Shudder.Services;
using Shudder.Vews;
using UnityEngine;
using Grid = Shudder.Models.Grid;

namespace Shudder.Factories
{
    public class GridFactory
    {
        private const float InnerRadiusCoefficient = 0.86f;

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
            
            if(level == -1)
                await BuilderGrid(grid, _hexogenGridConfig, isMenu);
            else
                await BuilderGrid(grid, _hexogenGridConfigs[level], isMenu);

            return grid;
        }
        
        public async UniTask BuilderGrid(Grid grid, HexogenGridConfig config, bool isMenu)
        {
            grid.Grounds = new Ground[config.Width, config.Height];
            
            var groundFactory = _container.Resolve<GroundFactory>();
            var offsetPosition = GetOffsetPosition(config);

            for (int z = 0; z < config.Height; z++)
            {
                for (int x = 0; x < config.Width; x++)
                {
                    GroundType groundType = GetGroundType(grid, config, x, z);

                    grid.Grounds[x,z] = groundFactory.Create(config, grid.Presenter.View.transform, x, z, offsetPosition, groundType, isMenu);

                    if (groundType == GroundType.Hole)
                        grid.Hole = grid.Grounds[x, z];
                }
            }
            await UniTask.Yield();

            if (isMenu)
                return;

            SetNeighbors(grid);
        }

        private Vector3 GetOffsetPosition(HexogenGridConfig config)
        {
            var rowOffset = config.Height % 2 * 0.5f;

            var x = (config.Width + rowOffset) * config.SpaceBetweenCells * 0.5f;
            var z = config.Height * config.SpaceBetweenCells * InnerRadiusCoefficient * 0.5f;
            var y = config.CaneraOffset;
            
            return _container.Resolve<CameraService>().Camera.transform.position - new Vector3(x, y, z);
        }

        private void SetNeighbors(Grid grid)
        {            
            for (int y = 0; y < grid.Grounds.GetLength(1); y++)
            {
                for (int x = 0; x < grid.Grounds.GetLength(0); x++)
                {
                    AddAllNeighbors(grid, y, x);
                }
            }
        }

        private GroundType GetGroundType(Grid grid, HexogenGridConfig config, int x, int y)
        {
            if (TryGetHole(grid, config, x, y))
                return GroundType.Hole;

            if (TryGetWall(config, x, y))
                return GroundType.Wall;

            if (TryGetPit(grid, config, x, y))
                return GroundType.Pit;

            return GetTileType();
        }

        private bool TryGetPit(Grid grid, HexogenGridConfig config, int x, int y)
        {
            if (grid.CountPit >= config.PitCount)
                return false;
            if (x < config.MinPitPositionForX - 1 || x >= config.MaxPitPositionForX) 
                return false;
            if (y < config.MinPitPositionForY - 1 || y >= config.MaxPitPositionForY)
                return false;
            if (!(Random.value < config.ChanceOfPit))
                return false;
            
            grid.CountPit++;
            return true;
        }

        private bool TryGetWall(HexogenGridConfig config, int x, int y)
        {
            if (x == 0 || x == config.Width - 1)
                return true;

            if (y == 0 || y == config.Height - 1)
                return true;
            
            return false;
        }
        
        private bool TryGetHole(Grid grid, HexogenGridConfig config, int x, int y)
        {
            var holePosition = CalculateHolePosition(config);
            return Mathf.Approximately(x, holePosition.x) && Mathf.Approximately(y, holePosition.y);
        }

        private Vector3 CalculateHolePosition(HexogenGridConfig config)
        {
            var xHole = Random.Range(config.MinHolePositionForX - 1, config.MaxHolePositionForX);
            var yHole = Random.Range(config.MinHolePositionForY - 1, config.MaxHolePositionForY);
            return new Vector2(xHole, yHole);
        }

        private GroundType GetTileType()
        {
            var type = GroundType.TileMedium;

            var r = Random.value;
            if (r > 0.7)
                type = GroundType.TileHigh;
            else if (r < 0.3)
                type = GroundType.TileLow;
            return type;
        }

        private void AddAllNeighbors(Grid grid, int y, int x)
        {
            var neighbors = FindNeighbors(x, y, grid);
            grid.Grounds[x, y].AddNeighbors(neighbors);
        }

        private List<Ground> FindNeighbors(int x, int y, Grid grid)
        {
            var neighbors = new List<Ground>();
            
            if (y % 2 != 0)
            {
                if (x - 1 >= 0)
                {
                    neighbors.Add(grid.Grounds[x - 1, y]);
                }

                if (x + 1 < grid.Grounds.GetLength(0))
                {
                    neighbors.Add(grid.Grounds[x + 1, y]);

                    if (y + 1 < grid.Grounds.GetLength(1))
                    {
                        neighbors.Add(grid.Grounds[x + 1, y + 1]);
                    }
                }

                if (y + 1 < grid.Grounds.GetLength(1))
                {
                    neighbors.Add(grid.Grounds[x, y + 1]);
                }

                if (x + 1 < grid.Grounds.GetLength(0) && y - 1 >= 0)
                {
                    neighbors.Add(grid.Grounds[x + 1, y - 1]);
                }

                if (y - 1 >= 0)
                {
                    neighbors.Add(grid.Grounds[x, y - 1]);
                }
            }
            else
            {
                if (x - 1 >= 0)
                {
                    neighbors.Add(grid.Grounds[x - 1, y]);

                    if (y + 1 < grid.Grounds.GetLength(1))
                    {
                        neighbors.Add(grid.Grounds[x - 1, y + 1]);
                    }

                    if (y - 1 >= 0)
                    {
                        neighbors.Add(grid.Grounds[x - 1, y - 1]);
                    }
                }

                if (y + 1 < grid.Grounds.GetLength(1))
                {
                    neighbors.Add(grid.Grounds[x, y + 1]);
                }

                if (x + 1 < grid.Grounds.GetLength(0))
                {
                    neighbors.Add(grid.Grounds[x + 1, y]);
                }

                if (y - 1 >= 0)
                {
                    neighbors.Add(grid.Grounds[x, y - 1]);
                }
            }

            return neighbors;
        }
    }
}