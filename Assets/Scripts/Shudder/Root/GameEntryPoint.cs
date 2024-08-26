using BaCon;
using Cysharp.Threading.Tasks;
using Shudder.Constants;
using Shudder.Data;
using Shudder.Events;
using Shudder.Factories;
using Shudder.Gameplay.Root;
using Shudder.Gameplay.Services;
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
        private readonly DIContainer _rootDiContainer;

        public GameEntryPoint()
        {
            _rootDiContainer = new DIContainer();
            
            Registration();
            Subscribe();
        }

        public void RunGame()
        {
#if UNITY_EDITOR
            var sceneName = SceneManager.GetActiveScene().name;

            switch (sceneName)
            {
                case SceneName.UI:
                    LoadAndStartTest();
                    return;
                case SceneName.MAIN_MENU:
                    LoadAndStartMainMenuScene();
                    break;
                case SceneName.TEST:
                    LoadAndStartTest();
                    return;
            }

            if (sceneName != SceneName.BOOT)
            {
                return;
            }
#endif
            LoadAndStartMainMenuScene();
        }

        private async void LoadAndStartTest()
        {
            var uiRoot = _rootDiContainer.Resolve<UIRootView>();
            uiRoot.ShowLoadingScreen();

            await LoadSceneAsync(SceneName.BOOT);
            await LoadSceneAsync(SceneName.MAIN_MENU);
           
            var mainMenu = Object.FindFirstObjectByType<MainMenuEntryPoint>();
            
            var di = new DIContainer(_rootDiContainer);
            mainMenu.Initialisation(di);
            
            await LoadSceneAsync(SceneName.BOOT);
            await LoadSceneAsync(SceneName.GAMEPLAY);

            var gameplay = Object.FindFirstObjectByType<GameplayEntryPoint>();
            var diContainer = new DIContainer(_rootDiContainer);
            await gameplay.Initialisation(diContainer);
            
            uiRoot.HideLoadingScreen();
        }

        private async void LoadAndStartMainMenuScene()
        {
            var uiRoot = _rootDiContainer.Resolve<UIRootView>();
            uiRoot.ShowLoadingScreen();

            await LoadSceneAsync(SceneName.BOOT);
            await LoadSceneAsync(SceneName.MAIN_MENU);
           
            var entryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();
            
            var diContainer = new DIContainer(_rootDiContainer);
            entryPoint.Initialisation(diContainer);
            uiRoot.HideLoadingScreen();
        }

        private async void LoadAndStartGameplayScene()
        {
            var uiRoot = _rootDiContainer.Resolve<UIRootView>();
            uiRoot.ShowLoadingScreen();

            await LoadSceneAsync(SceneName.BOOT);
            await LoadSceneAsync(SceneName.GAMEPLAY);

            var gameplay = Object.FindFirstObjectByType<GameplayEntryPoint>();
            var diContainer = new DIContainer(_rootDiContainer);
            await gameplay.Initialisation(diContainer);
            
            uiRoot.HideLoadingScreen();
        }

        private void Registration()
        {
            var cameraFollow = new CameraFollowFactory().Create();
            _rootDiContainer.RegisterInstance(new CameraService(cameraFollow));
        
            var uiRoot = CreateUIRoot();
            _rootDiContainer.RegisterInstance(uiRoot);    
            
            var eventBus = new EventBus();
            _rootDiContainer.RegisterInstance((ITriggerOnlyEventBus)eventBus);
            _rootDiContainer.RegisterInstance((IReadOnlyEventBus)eventBus);

            _rootDiContainer.RegisterInstance(new InputService());
            _rootDiContainer.RegisterInstance(new SfxService(_rootDiContainer));
            _rootDiContainer.RegisterInstance(new AnimationHeroService());
            _rootDiContainer.RegisterInstance(new RotationService());
            _rootDiContainer.RegisterInstance(new JumpService(_rootDiContainer));
            _rootDiContainer.RegisterInstance(new CoinService(_rootDiContainer));
            _rootDiContainer.RegisterInstance(new LeaderBoardsService(_rootDiContainer));
            _rootDiContainer.RegisterInstance(new StorageService(_rootDiContainer));
            _rootDiContainer.RegisterInstance(new ShopService(_rootDiContainer));
            _rootDiContainer.RegisterInstance(new SettingService(_rootDiContainer));
            _rootDiContainer.RegisterInstance(new RewardService(_rootDiContainer));

        }

        private UIRootView CreateUIRoot()
        {
            var prefabUIRoot = Resources.Load<UIRootView>(GameConstant.UiRootPath);
            var uiRoot = Object.Instantiate(prefabUIRoot);
            Object.DontDestroyOnLoad(uiRoot.gameObject);
            return uiRoot;
        }

        private void Subscribe()
        {
            var readOnlyEventBus = _rootDiContainer.Resolve<IReadOnlyEventBus>();

            readOnlyEventBus.UnSubscribe();
            readOnlyEventBus.StartGameplayScene.AddListener(OnLoadAndStartGameplayScene);
            readOnlyEventBus.GoMenu.AddListener(OnLoadAndStartMainMenuScene);
        }

        private void OnLoadAndStartMainMenuScene()
        {
            Subscribe();
            LoadAndStartMainMenuScene();
        }

        private void OnLoadAndStartGameplayScene()
        {
            Subscribe();
            LoadAndStartGameplayScene();
        }

        private async UniTask LoadSceneAsync(string sceneName)
        {
            await SceneManager.LoadSceneAsync(sceneName);
        }
    }
}
