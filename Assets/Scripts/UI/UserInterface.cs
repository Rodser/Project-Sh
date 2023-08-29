﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class UserInterface : MonoBehaviour
    {
        [field: SerializeField] public Menu MenuPanel  { get; private set; }
        [field: SerializeField] public Setting OptionPanel  { get; private set; }
        [field: SerializeField] public EventPanel EventPanel  { get; private set; }
        [field: SerializeField] public Hud HudPanel  { get; private set; }

        private Action<bool> _notifyEvent;
        private UnityAction _startLevelEvent;
        private InputSystem _input;

        private void Awake()
        {
            ReplaceMenu(true);
            EventPanel.gameObject.SetActive(false);
            OptionPanel.gameObject.SetActive(false);
        }

        public void Set(InputSystem input, UnityAction startLevel, Action<bool> notify, Action<int> changeCoin)
        {
            _input = input;
            
            MenuPanel.Subscribe(GoLevel, GoBack, GoOption);
            HudPanel.Subscribe(GoMenu, changeCoin);
            EventPanel.Subscribe(GoLevel);
            OptionPanel.Subscribe(GoBackWithoutOption);

            _startLevelEvent += startLevel;
            _notifyEvent += notify;
        }

        public void Notify(bool isVictory = true)
        {
            _notifyEvent?.Invoke(isVictory);
            _input.Disable();
            EventPanel.gameObject.SetActive(true);
            EventPanel.Notify(isVictory);
        }

        private void GoLevel()
        {
            ReplaceMenu(false);
            EventPanel.gameObject.SetActive(false);
            _startLevelEvent?.Invoke();
            _input.Enable();
        }

        private void GoMenu()
        {
            _input.Disable();
            ReplaceMenu(true);
            MenuPanel.ActivateBackButton();
        }

        private void GoOption()
        {
            OptionPanel.gameObject.SetActive(true);
        }

        private void GoBack()
        {
            ReplaceMenu(false);
            _input.Enable();
        }
        
        private void GoBackWithoutOption()
        {
            OptionPanel.gameObject.SetActive(false);
        }
        
        private void ReplaceMenu(bool isMenu)
        {
            HudPanel.gameObject.SetActive(!isMenu);
            MenuPanel.gameObject.SetActive(isMenu);
        }
    }
}