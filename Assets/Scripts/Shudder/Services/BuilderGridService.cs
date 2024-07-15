using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DI;
using Shudder.Configs;
using Shudder.Constants;
using Shudder.Factories;
using Shudder.Models;
using UnityEngine;
using Grid = Shudder.Models.Grid;

namespace Shudder.Services
{
    public class BuilderGridService
    {
        private readonly DIContainer _container;
        private Grid _grid;
        private HexogenGridConfig _config;
        private GroundFactory _groundFactory;
        private Vector3 _offsetPosition;
        private bool _isMenu;

        public BuilderGridService(DIContainer container)
        {
            _container = container;
        }

        public async UniTask<BuilderGridService> CreateGrounds(Grid grid, HexogenGridConfig config, bool isMenu)
        {
            _grid = grid;
            _config = config;
            _isMenu = isMenu;
            _grid.Grounds = new Ground[config.Width, config.Height];
            _offsetPosition = GetOffsetPosition(config);
            _groundFactory = _container.Resolve<GroundFactory>();

            for (int z = 0; z < config.Height; z++)
            {
                for (int x = 0; x < config.Width; x++)
                {
                    GroundType groundType = GetGroundType();

                    grid.Grounds[x, z] = await _groundFactory.Create(config, _grid.Presenter.View.transform, x, z,
                        _offsetPosition, groundType, isMenu);
                }
            }
            Debug.Log("Create Grounds");
            return this;
        }
        
        // public static implicit operator Grid(BuilderGridService builder)
        // {
        //     return builder._grid;
        // }

        public async UniTask<BuilderGridService> EstablishPit()
        {
            for (int y = 0; y < _grid.Grounds.GetLength(1); y++)
            {
                for (int x = 0; x < _grid.Grounds.GetLength(0); x++)
                {
                    if (!TryGetPit(x, y)) 
                        continue;
                    
                    _grid.Grounds[x, y].GroundType = GroundType.Pit;
                    Object.Destroy(_grid.Grounds[x, y].Presenter.View.gameObject);
                    _grid.Grounds[x, y].Presenter.View = null;
                    await UniTask.Yield();
                }
            }
            Debug.Log("Establish Pit");

            return this;
        }

        public async UniTask<BuilderGridService> EstablishHole()
        {
            for (int y = 0; y < _grid.Grounds.GetLength(1); y++)
            {
                for (int x = 0; x < _grid.Grounds.GetLength(0); x++)
                {
                    if (!TryGetHole(x, y) || _grid.Hole != null) 
                        continue;

                    if (_grid.Grounds[x, y] != null) 
                        Object.Destroy(_grid.Grounds[x, y].Presenter.View.gameObject);
                    
                    _grid.Grounds[x, y] = await _groundFactory
                        .Create(_config, _grid.Presenter.View.transform, x,y, _offsetPosition, GroundType.Hole, _isMenu);
                    
                    _grid.Hole = _grid.Grounds[x, y];
                }
            }
            Debug.Log("Establish Hole");
            return this;
        }
        
        public async UniTask<BuilderGridService>  SetNeighbors()
        {
            for (int y = 0; y < _grid.Grounds.GetLength(1); y++)
            {
                for (int x = 0; x < _grid.Grounds.GetLength(0); x++)
                {
                    AddAllNeighbors(_grid, y, x);
                    await UniTask.Yield();
                }
            }
            Debug.Log("Set Neighbors");
            return this;
        }

        public async UniTask<Grid> GetBuild()
        {
            await UniTask.Yield();
            return _grid;
        }

        private Vector3 GetOffsetPosition(HexogenGridConfig config)
        {
            var rowOffset = config.Height % 2 * 0.5f;

            var x = (config.Width + rowOffset) * config.SpaceBetweenCells * 0.5f;
            var z = config.Height * config.SpaceBetweenCells * Coefficient.InnerRadiusCoefficient * 0.5f;
            var y = config.CaneraOffset;

            return _container.Resolve<CameraService>().CameraFollow.Presenter.View.transform.position - new Vector3(x, y, z);
        }
      
        private bool TryGetPit(int x, int y)
        {
            if (_grid.CountPit >= _config.PitCount)
                return false;
            if (x < _config.MinPitPositionForX - 1 || x >= _config.MaxPitPositionForX)
                return false;
            if (y < _config.MinPitPositionForY - 1 || y >= _config.MaxPitPositionForY)
                return false;
            if (!(Random.value < _config.ChanceOfPit))
                return false;

            _grid.CountPit++;
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

        private bool TryGetHole(int x, int y)
        {
            var holePosition = CalculateHolePosition(_config);
            return Mathf.Approximately(x, holePosition.x) && Mathf.Approximately(y, holePosition.y);
        }

        private Vector3 CalculateHolePosition(HexogenGridConfig config)
        {
            var xHole = Random.Range(config.MinHolePositionForX - 1, config.MaxHolePositionForX);
            var yHole = Random.Range(config.MinHolePositionForY - 1, config.MaxHolePositionForY);
            return new Vector2(xHole, yHole);
        }

        private GroundType GetGroundType()
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