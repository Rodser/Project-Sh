using Cysharp.Threading.Tasks;
using GameApp.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameApp.Root
{
    public class GameEntryPoint
    {
        private readonly UIRootView _uiRoot;

        public GameEntryPoint()
        {
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
                LoadAndStartGameplayScene();
                return;
            }

            // if (sceneName == SceneName.MAIN_MENU)
            // {
            //     return;
            // }

            if (sceneName != SceneName.BOOT)
            {
                return;
            }
#endif
            LoadAndStartGameplayScene();
        }
        
        private async UniTask LoadAndStartGameplayScene()
        {
            _uiRoot.ShowLoadingScreen();

            await LoadSceneAsync(SceneName.BOOT);
            await LoadSceneAsync(SceneName.GAMEPLAY);

            _uiRoot.HideLoadingScreen();
        }
        
        private async UniTask LoadSceneAsync(string sceneName)
        {
            SceneManager.LoadSceneAsync(sceneName);
            await UniTask.Yield();
        }
    }
}