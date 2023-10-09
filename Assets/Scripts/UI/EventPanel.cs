using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class EventPanel : MonoBehaviour
    {
        [field: SerializeField] public Button NextButton { get; private set; }
        [field: SerializeField] public Button ExitButton { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Message  { get; private set; }

        public void Subscribe(UnityAction goLevel)
        {
            NextButton.onClick.AddListener(goLevel);
            ExitButton.onClick.AddListener(OnExit);
        }
        
        public void Notify(bool isVictory = true)
        {
             Message.text = isVictory ? "Победа" : "Проиграл";
             NextButton.GetComponentInChildren<TextMeshProUGUI>().text = isVictory ? "Вперед" : "Заново";
        }

        private void OnExit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
    }
}