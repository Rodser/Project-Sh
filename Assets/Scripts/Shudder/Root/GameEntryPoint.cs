using Cysharp.Threading.Tasks;
using DI;
using Shudder.Constants;
using Shudder.Events;
using Shudder.Gameplay.Root;
using Shudder.Gameplay.Services;
using Shudder.MainMenu.Root;
using Shudder.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shudder.Root
{
    public class GameEntryPoint
    {
        private readonly DIContainer _container;

        public GameEntryPoint()
        {
            _container = new DIContainer();
            _container.RegisterSingleton(c => new InputService());
            
            var eventBus = new EventBus();
            _container.RegisterInstance(eventBus);
            _container.RegisterInstance((IReadOnlyEventBus)eventBus);
            
            var uiRoot = CreateUIRoot();
            _container.RegisterInstance(uiRoot);

            _container.Resolve<IReadOnlyEventBus>().StartGameplayScene.AddListener(LoadAndStartGameplayScene);
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
            gameplay.Initialisation(_container);
            
            uiRoot.HideLoadingScreen();
        }
        
        private UIRootView CreateUIRoot()
        {
            var prefabUIRoot = Resources.Load<UIRootView>("UIRoot");
            var uiRoot = Object.Instantiate(prefabUIRoot);
            Object.DontDestroyOnLoad(uiRoot.gameObject);
            return uiRoot;
        }
        
        private async UniTask LoadSceneAsync(string sceneName)
        {
            await SceneManager.LoadSceneAsync(sceneName);
        }
    }
}
