using System.Collections;
using DI;
using Shudder.Gameplay.Root;
using Shudder.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Coroutine = Utils.Coroutine;

namespace Shudder.Root
{
    public class GameEntryPoint
    {
        private readonly DIContainer _container;

        public GameEntryPoint()
        {
            _container = new DIContainer();
            
            var coroutine = new GameObject("[COROUTINE]").AddComponent<Coroutine>();
            Object.DontDestroyOnLoad(coroutine.gameObject);
            
            _container.RegisterInstance(coroutine);
            
            var prefabUIRoot = Resources.Load<UIRootView>("UIRoot");
            var uiRoot = Object.Instantiate(prefabUIRoot);
            Object.DontDestroyOnLoad(uiRoot.gameObject);  
           
            _container.RegisterInstance(uiRoot);
        }

        public void RunGame()
        {
#if UNITY_EDITOR
            var sceneName = SceneManager.GetActiveScene().name;

            if (sceneName == SceneName.GAMEPLAY)
            {
                _container.Resolve<Coroutine>().StartCoroutine(LoadAndStartGameplayScene());
                return;
            }

            // if (sceneName == SceneName.MAIN_MENU)

            if (sceneName != SceneName.BOOT)
            {
                return;
            }
#endif
            _container.Resolve<Coroutine>().StartCoroutine(LoadAndStartGameplayScene());
        }

        private IEnumerator LoadAndStartGameplayScene()
        {
            var uiRoot = _container.Resolve<UIRootView>();
            var coroutine = _container.Resolve<Coroutine>();
            
            uiRoot.ShowLoadingScreen();

            coroutine.StartCoroutine(LoadSceneAsync(SceneName.BOOT));
            coroutine.StartCoroutine(LoadSceneAsync(SceneName.GAMEPLAY));

            yield return new WaitForSeconds(1);

            var gameplay = Object.FindFirstObjectByType<GameplayEntryPoint>();
            gameplay.Initialisation(_container);
            
            uiRoot.HideLoadingScreen();
        }
        
        private IEnumerator LoadSceneAsync(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
        }
    }
}