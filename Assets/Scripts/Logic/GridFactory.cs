using Cysharp.Threading.Tasks;
using Rodser.Config;
using Rodser.Model;
using UnityEngine;

namespace Rodser.Logic
{
    public class GridFactory
    {
        private readonly HexGridConfig _hexGridConfig;
        private int _countPit = 0;
        private Vector2 _holePosition;

        public GridFactory(HexGridConfig hexGridConfig)
        {
            _hexGridConfig = hexGridConfig;
        }

        public async UniTask<HexGrid> Create()
        {
            HexGrid grid = new HexGrid();

            CalculateHolePosition();
            var hex = new GameObject("HexogenGrid");            
            grid.Grounds = await BuilderGrid(hex.transform);
            grid.Initiation();
            return grid;
        }

        private async UniTask<Ground[,]> BuilderGrid(Transform parent)
        {
            Ground[,] grounds = new Ground[_hexGridConfig.Width, _hexGridConfig.Height];
            GroundFactory groundFactory = new GroundFactory(_hexGridConfig, parent);

            for (int z = 0; z < _hexGridConfig.Height; z++)
            {
                for (int x = 0; x < _hexGridConfig.Width; x++)
                {
                    GroundType groundType = GetGroundType(x, z);                    

                    grounds[x, z] = groundFactory.Create(x, z, groundType);
                    await UniTask.Delay(1);
                }
            }
            
            return grounds;
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
            if(_countPit >= _hexGridConfig.PitCount) 
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
            var xHole = Random.Range(_hexGridConfig.MinHolePositionForX, _hexGridConfig.MaxHolePositionForX);
            var yHole = Random.Range(_hexGridConfig.MinHolePositionForY, _hexGridConfig.MaxHolePositionForY);
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
    }
}