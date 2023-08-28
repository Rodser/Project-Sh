using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Core
{
    public class HUD : MonoBehaviour
    {
        [field: SerializeField, Header("Buttons")] public Button StartButton { get; private set; }    
        [field: SerializeField] public Button BackButton { get; private set; }    
        [field: SerializeField] public Button OptionButton { get; private set; }    
        [field: SerializeField] public Button ExitButton { get; private set; }    
        [field: SerializeField] public Button Exit2Button { get; private set; }    
        [field: SerializeField] public Button NextButton { get; private set; }    
        [field: SerializeField] public Button PauseButton { get; private set; }    
        [field: SerializeField, Header("Panels")] public RectTransform PanelMainMenu  { get; private set; }
        [field: SerializeField] public RectTransform PanelOptionMenu  { get; private set; }
        [field: SerializeField] public RectTransform PanelMessage  { get; private set; }
        [field: SerializeField] public RectTransform PanelHud  { get; private set; }
        [field: SerializeField, Header("Texts")] public TextMeshProUGUI Message  { get; private set; }

        public Action<bool> NotifyEvent;
        private InputSystem _input;

        private void Awake()
        {         
            ReplaceMenu(true);
            StartButton.onClick.AddListener(StartLevel);
            BackButton.onClick.AddListener(GoBack);
            ExitButton.onClick.AddListener(Exit);
            PauseButton.onClick.AddListener(GoMenu);
            NextButton.onClick.AddListener(StartLevel);
            Exit2Button.onClick.AddListener(Exit);
           
            PanelMessage.gameObject.SetActive(false);
            BackButton.gameObject.SetActive(false);
        }

        public void Notify(bool isVictory = true)
        {
            NotifyEvent?.Invoke(isVictory);
            PanelMessage.gameObject.SetActive(true);
            Message.text = isVictory ? "Победа" : "Проиграл";
            NextButton.GetComponentInChildren<TextMeshProUGUI>().text = isVictory ? "Вперед" : "Заново";
        }

        public void Set(InputSystem input, UnityAction startLevel, Action<bool> notify)
        {
            _input = input;
            StartButton.onClick.AddListener(startLevel);
            NextButton.onClick.AddListener(startLevel);
            NotifyEvent += notify;
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
            _input.Enable();
            ReplaceMenu(false);    
            PanelMessage.gameObject.SetActive(false);
        }

        private void GoMenu()
        {
            BackButton.gameObject.SetActive(true);
            _input.Disable();
            ReplaceMenu(true);
        }

        private void GoBack()
        {
            _input.Enable();
            ReplaceMenu(false);
        }

        private void ReplaceMenu(bool isMenu)
        {
            PanelHud.gameObject.SetActive(!isMenu);
            PanelMainMenu.gameObject.SetActive(isMenu);
        }
    }
}