using System.Collections;
using Shudder.Gameplay.Root;
using Shudder.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Coroutine = Utils.Coroutine;

namespace Shudder.Root
{
    public class GameEntryPoint
    {
        private readonly UIRootView _uiRoot;
        private readonly Coroutine _coroutine;

        public GameEntryPoint()
        {
            _coroutine = new GameObject("[COROUTINE]").AddComponent<Coroutine>();
            Object.DontDestroyOnLoad(_coroutine.gameObject);
            
            var prefabUIRoot = Resources.Load<UIRootView>("UIRoot");
            _uiRoot = Object.Instantiate(prefabUIRoot);
            Object.DontDestroyOnLoad(_uiRoot.gameObject);    
        }
        
        public void RunGame()
        {
#if UNITY_EDITOR
            var sceneName = SceneManager.GetActiveScene().name;

            if(sceneName == SceneName.GAMEPLAY)
            {
                _coroutine.StartCoroutine(LoadAndStartGameplayScene());
                return;
            }

            // if (sceneName == SceneName.MAIN_MENU)

            if (sceneName != SceneName.BOOT)
            {
                return;
            }
#endif
            _coroutine.StartCoroutine(LoadAndStartGameplayScene());
        }
        
        private IEnumerator LoadAndStartGameplayScene()
        {
            _uiRoot.ShowLoadingScreen();

            _coroutine.StartCoroutine(LoadSceneAsync(SceneName.BOOT));
            _coroutine.StartCoroutine(LoadSceneAsync(SceneName.GAMEPLAY));

            yield return new WaitForSeconds(1);

            var entryPoint = Object.FindFirstObjectByType<GameplayEntryPoint>();
            entryPoint.Run();
            
            _uiRoot.HideLoadingScreen();
        }
        
        private IEnumerator LoadSceneAsync(string sceneName)
        {
            var operation = SceneManager.LoadSceneAsync(sceneName);

            while (operation != null && operation.progress < 0.9f)
            {
                yield return null;
            }
            
            yield return null;
        }
    }
}