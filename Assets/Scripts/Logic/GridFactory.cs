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
            var hex = new GameObject("HexGrid");
            grid.Grounds = await BuilderGrid(hexGridConfig, hex.transform);
            return grid;
        }

        private async UniTask<Ground[]> BuilderGrid(HexGridConfig hexGridConfig, Transform parent)
        {
            Ground[] grounds = new Ground[hexGridConfig.Height * hexGridConfig.Width];
            GroundFactory groundFactory = new GroundFactory(hexGridConfig, parent);

            int n = 0;
            for (int z = 0; z < hexGridConfig.Height; z++)
            {
                for (int x = 0; x < hexGridConfig.Width; x++)
                {
                    grounds[n++] = groundFactory.Create(x, z);
                    await UniTask.Delay(10);
                }
            }

            return grounds;
        }
    }
}