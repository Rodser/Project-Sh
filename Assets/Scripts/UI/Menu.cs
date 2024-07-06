using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class Menu : MonoBehaviour, IInterface
    {
        [field: SerializeField] public Button StartButton { get; private set; }    
        [field: SerializeField] public Button BackButton { get; private set; }    
        [field: SerializeField] public Button OptionButton { get; private set; }    
        [field: SerializeField] public Button ExitButton { get; private set; }

        public void Subscribe(UnityAction goLevel, UnityAction goBack, UnityAction goOption)
        {
            BackButton.gameObject.SetActive(false);
            
            StartButton.onClick.AddListener(goLevel);
            BackButton.onClick.AddListener(goBack);
            OptionButton.onClick.AddListener(goOption);
            ExitButton.onClick.AddListener(ExitGame);
        }

        public void ActivateBackButton()
        {
            BackButton.gameObject.SetActive(true);
        }
        
        private void ExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
    }
}