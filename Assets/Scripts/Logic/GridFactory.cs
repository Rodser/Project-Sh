using Cysharp.Threading.Tasks;
using Model;
using Rodser.Config;
using UnityEngine;

namespace Logic
{
    public class GridFactory
    {
        private readonly HexogenGridConfig _hexGridConfig;

        public GridFactory(HexogenGridConfig hexGridConfig)
        {
            _hexGridConfig = hexGridConfig;
        }

        public async UniTask<HexogenGrid> Create(Transform body, bool isMenu = false)
        {
            HexogenGrid grid = new HexogenGrid(_hexGridConfig, body);
            await grid.BuilderGrid(isMenu);

            return grid;
        }
    }
}