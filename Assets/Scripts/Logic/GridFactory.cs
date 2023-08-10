using Cysharp.Threading.Tasks;
using Rodser.Config;
using Rodser.Model;
using UnityEngine;

namespace Rodser.Logic
{
    public class GridFactory
    {
        private readonly HexogenGridConfig _hexGridConfig;

        public GridFactory(HexogenGridConfig hexGridConfig)
        {
            _hexGridConfig = hexGridConfig;
        }

        public async UniTask<HexogenGrid> Create(bool isMenu = false)
        {
            string name = isMenu ? "MenuGrid" : "HexogenGrid";
            var hex = new GameObject(name);            

            HexogenGrid grid = new HexogenGrid(_hexGridConfig, hex.transform);
            await grid.BuilderGrid(isMenu);

            return grid;
        }
    }
}