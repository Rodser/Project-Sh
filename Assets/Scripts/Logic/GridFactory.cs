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
            var hex = new GameObject("HexogenGrid");            
            HexGrid grid = new HexGrid(_hexGridConfig, hex.transform);
            await grid.BuilderGrid();
            return grid;
        }
    }
}