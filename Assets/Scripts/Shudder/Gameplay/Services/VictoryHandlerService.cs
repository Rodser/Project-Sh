using BaCon;
using Shudder.Data;
using Shudder.Gameplay.Root;
using Shudder.UI;
using UnityEngine;

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
            game.UpLevel();
            Debug.Log("Has Victory, level Up");
            _container.Resolve<StorageService>().SaveProgress(game.Progress);
            await _container.Resolve<LevelLoadingService>().DestroyLevelAsync();
            await _container.Resolve<LevelLoadingService>().LoadAsync();
            _container.Resolve<UIRootView>().HideLoadingScreen();
        }
    }
}