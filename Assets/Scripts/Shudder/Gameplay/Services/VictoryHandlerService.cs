using Cysharp.Threading.Tasks;
using DI;
using Shudder.Configs;
using Shudder.Gameplay.Root;

namespace Shudder.Gameplay.Services
{
    public class VictoryHandlerService
    {
        private readonly DIContainer _container;
        private readonly GameConfig _gameConfig;

        public VictoryHandlerService(DIContainer container, GameConfig gameConfig)
        {
            _container = container;
            _gameConfig = gameConfig;
        }

        public async void HasVictory(Game game)
        {
            await UniTask.Delay(500);
            if(game.CurrentLevel < _gameConfig.LevelGridConfigs.Length - 1)
                game.CurrentLevel++;
            await _container.Resolve<LevelLoadingService>().LoadAsync(game);
            game.Run();
        }
    }
}