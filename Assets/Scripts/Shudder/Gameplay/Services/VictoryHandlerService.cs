using BaCon;
using Cysharp.Threading.Tasks;
using Shudder.Data;
using Shudder.Gameplay.Root;
using Shudder.UI;

namespace Shudder.Gameplay.Services
{
    public class VictoryHandlerService
    {
        private readonly DIContainer _container;

        public VictoryHandlerService(DIContainer container)
        {
            _container = container;
        }

        public async void HasVictory(Game game)
        {
            await UniTask.Delay(500);
            game.UpLevel();
            _container.Resolve<StorageService>().SaveProgress(game.Progress);
            await _container.Resolve<LevelLoadingService>().LoadAsync();
            _container.Resolve<UIRootView>().HideLoadingScreen();
        }
    }
}