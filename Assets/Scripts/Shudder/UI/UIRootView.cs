using UnityEngine;

namespace Shudder.UI
{
    public class UIRootView : MonoBehaviour
    {
        [SerializeField] private Transform _uiSceneContainer;
        [SerializeField] private LoadingScreenView _loadingScreenView;
        
        private void Awake()
        {
            HideLoadingScreen();
        }

        public void ShowLoadingScreen()
        {
            _loadingScreenView.gameObject.SetActive(true);
        }

        public void HideLoadingScreen()
        {
            _loadingScreenView.gameObject.SetActive(false);
        }

        public void ChangeSceneUI(GameObject sceneUI)
        {
            CleanSceneUI();
            AttachUI(sceneUI);
        }
        
        public void AttachUI(GameObject ui)
        {
            ui.transform.SetParent(_uiSceneContainer, false);
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