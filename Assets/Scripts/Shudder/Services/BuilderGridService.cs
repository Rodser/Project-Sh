using System.Collections.Generic;
using BaCon;
using Cysharp.Threading.Tasks;
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
        private GroundFactory _groundFactory;
        private CameraService _cameraService;
        
        private Grid _grid;
        private GridConfig _config;
        private Vector3 _offsetPosition;
        private Ground[,] _cells;
        private bool _isMenu;

        public BuilderGridService()
        {
        }
        
        public void Construct(CameraService cameraService, GroundFactory groundFactory)
        {
            _cameraService = cameraService;
            _groundFactory = groundFactory;
        }
        
        public  BuilderGridService CreateGrounds(Grid grid, GridConfig config, bool isMenu)
        {
            _grid = grid;
            _config = config;
            _isMenu = isMenu;
            _grid.Grounds = new Ground[config.Width, config.Height];
            _cells = new Ground[config.Width, config.Height];
            _offsetPosition = GetOffsetPosition(config);

            for (int z = 0; z < config.Height; z++)
            {
                for (int x = 0; x < config.Width; x++)
                {
                    _cells[x, z] = new Ground(GetGroundType());
                }
            }
            return this;
        }

        public BuilderGridService EstablishWall()
        {
            for (int y = 0; y < _grid.Grounds.GetLength(1); y++)
            {
                for (int x = 0; x < _grid.Grounds.GetLength(0); x++)
                {
                    if (!TryGetWall(x, y)) 
                        continue;
                    
                    _cells[x, y].GroundType = GroundType.Wall;
                }
            }

            return this;
        }
        
        public BuilderGridService EstablishPit()
        {
            for (int y = 0; y < _grid.Grounds.GetLength(1); y++)
            {
                for (int x = 0; x < _grid.Grounds.GetLength(0); x++)
                {
                    if (!TryGetPit(x, y)) 
                        continue;
                    
                    _cells[x, y].GroundType = GroundType.Pit;
                }
            }

            return this;
        }

        public BuilderGridService EstablishPortal()
        {
            var v = CalculateHolePosition(_config);
                    _cells[v.x, v.y].GroundType = GroundType.Portal;
                
            return this;
        }

        public async UniTask<Grid> GetBuildAsync()
        {
            for (int z = 0; z < _config.Height; z++)
            {
                for (int x = 0; x < _config.Width; x++)
                {
                        _grid.Grounds[x, z] = await _groundFactory
                            .CreateAsync(_config, _grid.Presenter.View.transform, x, z, _offsetPosition,  _cells[x, z].GroundType, _isMenu);
                        
                        if (_cells[x, z].GroundType == GroundType.Portal)
                            _grid.Portal = _grid.Grounds[x, z];
                }
            }

            await SetNeighbors();
            Debug.Log("Create Grounds");
            return _grid;
        }
        
        public void Build()
        {
            for (int z = 0; z < _config.Height; z++)
            {
                for (int x = 0; x < _config.Width; x++)
                {
                    _grid.Grounds[x, z] = _groundFactory
                        .Create(_config, _grid.Presenter.View.transform, x, z, _offsetPosition,  _cells[x, z].GroundType);
                        
                    if (_cells[x, z].GroundType == GroundType.Portal)
                        _grid.Portal = _grid.Grounds[x, z];
                }
            }
            Debug.Log("Created Grounds");
        }

        private async UniTask SetNeighbors()
        {
            for (int y = 0; y < _grid.Grounds.GetLength(1); y++)
            {
                for (int x = 0; x < _grid.Grounds.GetLength(0); x++)
                {
                    AddAllNeighbors(_grid, y, x);
                    await UniTask.Yield();
                }
            }
        }

        private Vector3 GetOffsetPosition(GridConfig config)
        {
            var rowOffset = config.Height % 2 * 0.5f;

            var x = (config.Width + rowOffset) * config.SpaceBetweenCells * 0.5f;
            var z = config.Height * config.SpaceBetweenCells * GameConstant.InnerRadiusCoefficient * 0.5f;
            var y = 0f; 

            return _cameraService.View.transform.position - new Vector3(x, y, z);
        }

        private bool TryGetPit(int x, int y)
        {
            if (_grid.CountPit >= _config.PitCount)
                return false;
            if (x < _config.PitPositionForWidth.x || x >= _config.PitPositionForWidth.y)
                return false;
            if (y < _config.PitPositionForHeight.x || y >= _config.PitPositionForHeight.y)
                return false;
            if (!(Random.value < _config.ChanceOfPit))
                return false;

            _grid.CountPit++;
            return true;
        }

        private bool TryGetWall(int x, int y)
        {
            if (x < _config.WallPositionForWidth.x || x >= _config.WallPositionForWidth.y)
                return false;
            if (y < _config.WallPositionForHeight.x || y >= _config.WallPositionForHeight.y)
                return false;
            
            return Random.value < _config.ChanceOfWall;
        }

        private Vector2Int CalculateHolePosition(GridConfig config)
        {
            var xHole = Random.Range(config.HolePositionForWidth.x - 1, config.HolePositionForWidth.y);
            var yHole = Random.Range(config.HolePositionForHeight.x - 1, config.HolePositionForHeight.y);
            return new Vector2Int(xHole, yHole);
        }

        private GroundType GetGroundType()
        {
            var type = GroundType.TileMedium;

            var r = Random.value;
            if (r > 0.5)
                type = GroundType.TileHigh;
            else if (r < 0.2)
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