using Cysharp.Threading.Tasks;
using Rodser.Config;
using Rodser.Model;
using UnityEngine;

namespace Rodser.Logic
{
    public class GridFactory
    {
        private readonly HexGridConfig _hexGridConfig;

        public GridFactory(HexGridConfig hexGridConfig)
        {
            _hexGridConfig = hexGridConfig;
        }

        public async UniTask<HexGrid> Create()
        {
            HexGrid grid = new HexGrid();
            var hex = new GameObject("HexogenGrid");
            grid.Grounds = await BuilderGrid(hex.transform);
            grid.Initiation();
            return grid;
        }

        private async UniTask<Ground[,]> BuilderGrid(Transform parent)
        {
            Ground[,] grounds = new Ground[_hexGridConfig.Width, _hexGridConfig.Height];
            GroundFactory groundFactory = new GroundFactory(_hexGridConfig, parent);
            var xHole = Random.Range(0, _hexGridConfig.Width);
            var zHole = Random.Range(_hexGridConfig.Height / 2, _hexGridConfig.Height);

            for (int z = 0; z < _hexGridConfig.Height; z++)
            {
                for (int x = 0; x < _hexGridConfig.Width; x++)
                {
                    GroundType groundType;
                    if (x == xHole && z == zHole)
                    {
                        groundType = GroundType.Hole;
                    }
                    else
                    {
                        groundType = GetGroundType(x, z);
                    }

                    grounds[x, z] = groundFactory.Create(x, z, groundType);
                    await UniTask.Delay(1);
                }
            }
            
            return grounds;
        }

        private GroundType GetGroundType(int x, int y)
        {
            GroundType type = GetTileType();
            if (x < Random.value * 5 && x > Random.value * 2)
            {
                if (y < Random.value * 10 && y > Random.value * 4)
                {
                    type = GroundType.Pit;
                }
            }

            return type;
        }

        private static GroundType GetTileType()
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