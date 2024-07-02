using UnityEngine;

namespace GameApp.UI
{
    public class UIRootView : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingScreen;
        [SerializeField] private Transform _uiSceneContainer;

        private void Awake()
        {
            HideLoadingScreen();
        }

        public void ShowLoadingScreen()
        {
            _loadingScreen.SetActive(true);
        }

        public void HideLoadingScreen()
        {
            _loadingScreen.SetActive(false);
        }

        public void AttachSceneUI(GameObject sceneUI)
        {
            CleanSceneUI();

            sceneUI.transform.SetParent(_uiSceneContainer, false);
        }

        private void CleanSceneUI()
        {
            var childCount = _uiSceneContainer.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Destroy(_uiSceneContainer.GetChild(i).gameObject);
            }
        }
    }
}