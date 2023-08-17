using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class HUD : MonoBehaviour
    {
        [field: SerializeField] public Button StartButton { get; private set; }    
        [field: SerializeField] public Button BackButton { get; private set; }    
        [field: SerializeField] public Button OptionButton { get; private set; }    
        [field: SerializeField] public Button ExitButton { get; private set; }    
        [field: SerializeField] public Button PauseButton { get; private set; }    
        [field: SerializeField] public RectTransform PanelMainMenu  { get; private set; }
        [field: SerializeField] public RectTransform PanelOptionMenu  { get; private set; }
        [field: SerializeField] public RectTransform PanelHud  { get; private set; }


        private void Awake()
        {         
            ReplaceMenu(true);
            StartButton.onClick.AddListener(StartLevel);
            BackButton.onClick.AddListener(GoBack);
            ExitButton.onClick.AddListener(Exit);
            PauseButton.onClick.AddListener(GoMenu);
            
            BackButton.gameObject.SetActive(false);
        }

        private void Exit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }

        private void StartLevel()
        {
            Debug.Log("Start");
            ReplaceMenu(false);
        }

        private void GoMenu()
        {
            BackButton.gameObject.SetActive(true);
            ReplaceMenu(true);
        }

        private void GoBack()
        {
            ReplaceMenu(false);
        }

        private void ReplaceMenu(bool isMenu)
        {
            PanelHud.gameObject.SetActive(!isMenu);
            PanelMainMenu.gameObject.SetActive(isMenu);
        }
    }
}