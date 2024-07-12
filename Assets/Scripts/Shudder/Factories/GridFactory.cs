using Cysharp.Threading.Tasks;
using DI;
using Shudder.Configs;
using Shudder.Models;
using UnityEngine;

namespace Shudder.Factories
{
    public class GridFactory
    {
        private readonly DIContainer _container;
        private readonly HexogenGridConfig[] _hexogenGridConfigs;
        private readonly HexogenGridConfig _hexogenGridConfig;

        public GridFactory(DIContainer container, HexogenGridConfig[] hexogenGridConfigs)
        {
            _container = container;
            _hexogenGridConfigs = hexogenGridConfigs;
        }
        
        public GridFactory(DIContainer container, HexogenGridConfig hexogenGridConfig)
        {
            _container = container;
            _hexogenGridConfig = hexogenGridConfig;
        }
        
        public async UniTask<HexogenGrid> Create(int level, Transform body, bool isMenu = false)
        {
            var grid = new HexogenGrid(_container, _hexogenGridConfigs[level], body);
            await grid.BuilderGrid(isMenu);

            return grid;
        }
              
        public async UniTask<HexogenGrid> Create(Transform body, bool isMenu = false)
        {
            var grid = new HexogenGrid(_container, _hexogenGridConfig, body);
            await grid.BuilderGrid(isMenu);

            return grid;
        }
    }
}