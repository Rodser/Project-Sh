using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class HUD : MonoBehaviour
    {
        [field: SerializeField] public Button StartButton { get; private set; }    
        [field: SerializeField] public Button OptionButton { get; private set; }    
        [field: SerializeField] public Button ExitButton { get; private set; }    
        [field: SerializeField] public RectTransform PanelMainMenu  { get; private set; }
        [field: SerializeField] public RectTransform PanelOptionMenu  { get; private set; }
        [field: SerializeField] public RectTransform PanelHud  { get; private set; }


        private void Awake()
        {
            StartButton.onClick.AddListener(StartLevel);
            ExitButton.onClick.AddListener(Exit);
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
            PanelMainMenu.gameObject.SetActive(false);
        }
    }
}