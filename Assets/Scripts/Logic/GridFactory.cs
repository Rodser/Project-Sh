using Cysharp.Threading.Tasks;
using Rodser.Config;
using Rodser.Model;
using UnityEngine;

namespace Rodser.Logic
{
    public class GridFactory
    {
        public async UniTask<HexGrid> Create(HexGridConfig hexGridConfig)
        {
            HexGrid grid = new HexGrid();
            var hex = new GameObject("HexogenGrid");
            grid.Grounds = await BuilderGrid(hexGridConfig, hex.transform);
            grid.Initiation();
            return grid;
        }

        private async UniTask<Ground[,]> BuilderGrid(HexGridConfig hexGridConfig, Transform parent)
        {
            Ground[,] grounds = new Ground[hexGridConfig.Width, hexGridConfig.Height];
            GroundFactory groundFactory = new GroundFactory(hexGridConfig, parent);

            for (int z = 0; z < hexGridConfig.Height; z++)
            {
                for (int x = 0; x < hexGridConfig.Width; x++)
                {
                    grounds[x, z] = groundFactory.Create(x, z);
                    //await UniTask.Delay(10);
                }
            }
            
            return grounds;
        }
    }
}