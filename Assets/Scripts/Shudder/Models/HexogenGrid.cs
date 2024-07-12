using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DI;
using Shudder.Configs;
using Shudder.Factories;
using Shudder.Services;
using UnityEngine;

namespace Shudder.Models
{
    public class HexogenGrid
    {
        private const float InnerRadiusCoefficient = 0.86f;

        private readonly DIContainer _container;
        private readonly HexogenGridConfig _hexGridConfig;
        private readonly Transform _parent;

        private int _countPit;
        private Vector2 _holePosition;
        private GroundFactory _groundFactory;

        public HexogenGrid(DIContainer container, HexogenGridConfig hexGridConfig, Transform parent)
        {
            _container = container;
            _hexGridConfig = hexGridConfig;
            _parent = parent;
        }

        public Ground[,] Grounds { get; private set; }
        public Ground Hole { get; private set; }
        public Vector3 OffsetPosition { get ; private set ; }

        internal async UniTask BuilderGrid(bool isMenu)
        {
            Initialize();
            CalculateHolePosition();

            for (int z = 0; z < _hexGridConfig.Height; z++)
            {
                for (int x = 0; x < _hexGridConfig.Width; x++)
                {
                    GroundType groundType = GetGroundType(x, z);

                    Ground ground = _groundFactory
                        .Create(_hexGridConfig, _parent, x, z, OffsetPosition, groundType, isMenu);
                    Grounds[x, z] = ground;

                    if (groundType == GroundType.Hole)
                        Hole = ground;
                }
            }

            await UniTask.Yield();

            if (isMenu)
                return;

            SetNeighbors();
        }

        private void Initialize()
        {
            Grounds = new Ground[_hexGridConfig.Width, _hexGridConfig.Height];
            _groundFactory = _container.Resolve<GroundFactory>();
            OffsetPosition = GetOffsetPosition();
        }

        private Vector3 GetOffsetPosition()
        {
            var rowOffset = _hexGridConfig.Height % 2 * 0.5f;

            var x = (_hexGridConfig.Width + rowOffset) * _hexGridConfig.SpaceBetweenCells * 0.5f;
            var z = _hexGridConfig.Height * _hexGridConfig.SpaceBetweenCells * InnerRadiusCoefficient * 0.5f;
            var y = _hexGridConfig.CaneraOffset;
            
            return _container.Resolve<CameraService>().Camera.transform.position - new Vector3(x, y, z);
        }

        private void SetNeighbors()
        {            
            for (int y = 0; y < Grounds.GetLength(1); y++)
            {
                for (int x = 0; x < Grounds.GetLength(0); x++)
                {
                    AddAllNeighbors(y, x);
                }
            }
        }

        private GroundType GetGroundType(int x, int y)
        {
            if (TryGetHole(x, y))
                return GroundType.Hole;

            if (TryGetWall(x, y))
                return GroundType.Wall;

            if (TryGetPit(x, y))
                return GroundType.Pit;

            return GetTileType();
        }

        private bool TryGetPit(int x, int y)
        {
            if (_countPit >= _hexGridConfig.PitCount)
                return false;
            if (x < _hexGridConfig.MinPitPositionForX - 1 || x >= _hexGridConfig.MaxPitPositionForX) 
                return false;
            if (y < _hexGridConfig.MinPitPositionForY - 1 || y >= _hexGridConfig.MaxPitPositionForY)
                return false;
            if (!(Random.value < _hexGridConfig.ChanceOfPit)) // * y / _hexGridConfig.Height))
                return false;
            
            _countPit++;
            return true;
        }

        private bool TryGetWall(int x, int y)
        {
            if (x == 0 || x == _hexGridConfig.Width - 1)
                return true;
            
            if (y == 0 || y == _hexGridConfig.Height - 1)
                return true;
            
            return false;
        }
        
        private bool TryGetHole(int x, int y)
        {
            return Mathf.Approximately(x, _holePosition.x) && Mathf.Approximately(y, _holePosition.y);
        }

        private void CalculateHolePosition()
        {
            var xHole = Random.Range(_hexGridConfig.MinHolePositionForX - 1, _hexGridConfig.MaxHolePositionForX);
            var yHole = Random.Range(_hexGridConfig.MinHolePositionForY - 1, _hexGridConfig.MaxHolePositionForY);
            _holePosition = new Vector2(xHole, yHole);
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

        private void AddAllNeighbors(int y, int x)
        {
            List<Ground> neighbors = new List<Ground>();
            FindNeighbors(x, y, neighbors);

            Grounds[x, y].AddNeighbors(neighbors);
        }

        private void FindNeighbors(int x, int y, List<Ground> neighbors)
        {
            if (y % 2 != 0)
            {
                if (x - 1 >= 0)
                {
                    neighbors.Add(Grounds[x - 1, y]);
                }

                if (x + 1 < Grounds.GetLength(0))
                {
                    neighbors.Add(Grounds[x + 1, y]);

                    if (y + 1 < Grounds.GetLength(1))
                    {
                        neighbors.Add(Grounds[x + 1, y + 1]);
                    }
                }

                if (y + 1 < Grounds.GetLength(1))
                {
                    neighbors.Add(Grounds[x, y + 1]);
                }

                if (x + 1 < Grounds.GetLength(0) && y - 1 >= 0)
                {
                    neighbors.Add(Grounds[x + 1, y - 1]);
                }

                if (y - 1 >= 0)
                {
                    neighbors.Add(Grounds[x, y - 1]);
                }
            }
            else
            {
                if (x - 1 >= 0)
                {
                    neighbors.Add(Grounds[x - 1, y]);

                    if (y + 1 < Grounds.GetLength(1))
                    {
                        neighbors.Add(Grounds[x - 1, y + 1]);
                    }
                    if (y - 1 >= 0)
                    {
                        neighbors.Add(Grounds[x - 1, y - 1]);
                    }
                }

                if (y + 1 < Grounds.GetLength(1))
                {
                    neighbors.Add(Grounds[x, y + 1]);
                }

                if (x + 1 < Grounds.GetLength(0))
                {
                    neighbors.Add(Grounds[x + 1, y]);
                }

                if (y - 1 >= 0)
                {
                    neighbors.Add(Grounds[x, y - 1]);
                }
            }                   
        }
    }
}