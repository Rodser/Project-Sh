using Cysharp.Threading.Tasks;
using Rodser.Config;
using Rodser.Logic;
using System.Collections.Generic;
using UnityEngine;

namespace Rodser.Model
{
    public class HexogenGrid
    {
        private readonly HexogenGridConfig _hexGridConfig;
        private readonly Transform _parent;

        private int _countPit;
        private Vector2 _holePosition;

        public HexogenGrid(HexogenGridConfig hexGridConfig, Transform parent)
        {
            _hexGridConfig = hexGridConfig;
            _parent = parent;
        }

        public Ground[,] Grounds { get; internal set; }
        public Ground Hole { get; private set; }

        internal async UniTask BuilderGrid(bool isMenu)
        {
            Grounds = new Ground[_hexGridConfig.Width, _hexGridConfig.Height];
            GroundFactory groundFactory = new GroundFactory(_hexGridConfig, _parent);

            CalculateHolePosition();

            for (int z = 0; z < _hexGridConfig.Height; z++)
            {
                for (int x = 0; x < _hexGridConfig.Width; x++)
                {
                    GroundType groundType = GetGroundType(x, z);

                    Ground ground = groundFactory.Create(x, z, groundType, isMenu);
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

            if (TryGetPit(x, y))
                return GroundType.Pit;

            return GetTileType();
        }

        private bool TryGetPit(int x, int y)
        {
            if (_countPit >= _hexGridConfig.PitCount)
                return false;

            if (x >= _hexGridConfig.MinPitPositionForX - 1 && x < _hexGridConfig.MaxPitPositionForX)
            {
                if (y >= _hexGridConfig.MinPitPositionForY - 1 && y < _hexGridConfig.MaxPitPositionForY)
                {
                    if (Random.value < _hexGridConfig.ChanceOfPit * y / _hexGridConfig.Height)
                    {
                        _countPit++;
                        return true;
                    }
                }
            }

            return false;
        }

        private bool TryGetHole(int x, int y)
        {
            if (x == _holePosition.x && y == _holePosition.y)
            {
                return true;
            }

            return false;
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
            if (y % 2 == 0)
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