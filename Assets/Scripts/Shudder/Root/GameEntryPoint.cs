using Cysharp.Threading.Tasks;
using DI;
using Shudder.Constants;
using Shudder.Events;
using Shudder.Gameplay.Root;
using Shudder.MainMenu.Root;
using Shudder.Services;
using Shudder.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Shudder.Root
{
    public class GameEntryPoint
    {
        private readonly DIContainer _container;

        public GameEntryPoint()
        {
            _container = new DIContainer();
            
            CreateAndRegisterUIRoot();
            
            RegisterEventBus();
            RegisterService();

            Subscribe();
        }

        public void RunGame()
        {
#if UNITY_EDITOR
            var sceneName = SceneManager.GetActiveScene().name;

            switch (sceneName)
            {
                case SceneName.GAMEPLAY:
                    LoadAndStartGameplayScene();
                    return;
                case SceneName.MAIN_MENU:
                    LoadAndStartMainMenuScene();
                    break;
            }

            if (sceneName != SceneName.BOOT)
            {
                return;
            }
#endif
            LoadAndStartMainMenuScene();
        }

        private async void LoadAndStartMainMenuScene()
        {
            var uiRoot = _container.Resolve<UIRootView>();
            uiRoot.ShowLoadingScreen();

            await LoadSceneAsync(SceneName.BOOT);
            await LoadSceneAsync(SceneName.MAIN_MENU);
           
            var entryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();
            entryPoint.Initialisation(_container);
            
            uiRoot.HideLoadingScreen();
        }

        private async void LoadAndStartGameplayScene()
        {
            var uiRoot = _container.Resolve<UIRootView>();
            uiRoot.ShowLoadingScreen();

            await LoadSceneAsync(SceneName.BOOT);
            await LoadSceneAsync(SceneName.GAMEPLAY);

            var gameplay = Object.FindFirstObjectByType<GameplayEntryPoint>();
            await gameplay.Initialisation(_container);
            
            uiRoot.HideLoadingScreen();
        }

        private UIRootView CreateUIRoot()
        {
            var prefabUIRoot = Resources.Load<UIRootView>("UIRoot");
            var uiRoot = Object.Instantiate(prefabUIRoot);
            Object.DontDestroyOnLoad(uiRoot.gameObject);
            return uiRoot;
        }

        private void Subscribe()
        {
            _container
                .Resolve<IReadOnlyEventBus>()
                .StartGameplayScene
                .AddListener(LoadAndStartGameplayScene);
        }

        private void CreateAndRegisterUIRoot()
        {
            var uiRoot = CreateUIRoot();
            _container.RegisterInstance(uiRoot);
        }

        private void RegisterEventBus()
        {
            var eventBus = new EventBus();
            _container.RegisterInstance((ITriggerOnlyEventBus)eventBus);
            _container.RegisterInstance((IReadOnlyEventBus)eventBus);
        }

        private void RegisterService()
        {
            _container.RegisterSingleton(c => new InputService());
        }

        private async UniTask LoadSceneAsync(string sceneName)
        {
            await SceneManager.LoadSceneAsync(sceneName);
        }
    }
}
