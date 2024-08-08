using Cysharp.Threading.Tasks;
using DI;
using Shudder.Configs;
using Shudder.Data;
using Shudder.Gameplay.Root;
using Shudder.UI;

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
            game.UpLevel();
            _container.Resolve<StorageService>().SaveProgress(game.Progress);
            await _container.Resolve<LevelLoadingService>().LoadAsync(game);
            _container.Resolve<UIRootView>().HideLoadingScreen();
            game.Run();
        }
    }
}