using Config;
using Cysharp.Threading.Tasks;
using Model;
using UnityEngine;

namespace Logic
{
    public class GridFactory
    {
        private readonly HexogenGridConfig[] _hexogensGridConfigs;
        private readonly HexogenGridConfig _hexogenGridConfig;

        public GridFactory(HexogenGridConfig[] hexogensGridConfigs)
        {
            _hexogensGridConfigs = hexogensGridConfigs;
        }
        
        public GridFactory(HexogenGridConfig hexGridConfig)
        {
            _hexogenGridConfig = hexGridConfig;
        }
        
        public async UniTask<HexogenGrid> Create(int level, Transform body, bool isMenu = false)
        {
            HexogenGrid grid = new HexogenGrid(_hexogensGridConfigs[level], body);
            await grid.BuilderGrid(isMenu);

            return grid;
        }
              
        public async UniTask<HexogenGrid> Create(Transform body, bool isMenu = false)
        {
            HexogenGrid grid = new HexogenGrid(_hexogenGridConfig, body);
            await grid.BuilderGrid(isMenu);

            return grid;
        }
    }
}