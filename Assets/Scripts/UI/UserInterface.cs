using System;
using Core;
using Shudder.Gameplay.Services;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class UserInterface : MonoBehaviour
    {
        [field: SerializeField] public Setting OptionPanel  { get; private set; }
        [field: SerializeField] public EventPanel EventPanel  { get; private set; }
        [field: SerializeField] public Hud HudPanel  { get; private set; }

        private Action<bool> _notifyEvent;
        private UnityAction _startLevelEvent;
        private InputService _input;
        private AudioSource _music;
        private AudioSource _clickSFX;

        private void Awake()
        {
            ReplaceMenu(true);
            EventPanel.gameObject.SetActive(false);
            OptionPanel.gameObject.SetActive(false);
        }

        public void Construct(InputService input, SoundFactory soundFactory, UnityAction startLevel, Action<bool> notify)
        {
            _input = input;
            _music = soundFactory.Create(SFX.Music);
            _clickSFX = soundFactory.Create(SFX.Click);

            EventPanel.Subscribe(GoLevel, soundFactory);
            OptionPanel.Subscribe(_music, GoBackWithoutOption);

            _startLevelEvent += startLevel;
            _notifyEvent += notify;
        }

        public void PlayMusic()
        {
            _music.Stop();
            _music.Play();
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
            _clickSFX.Play();

            ReplaceMenu(false);
            EventPanel.gameObject.SetActive(false);
            _startLevelEvent?.Invoke();
            _input.Enable();
        }

        private void GoMenu()
        {
            _clickSFX.Play();

            _input.Disable();
            ReplaceMenu(true);
        }

        private void GoBack()
        {
            _clickSFX.Play();

            ReplaceMenu(false);
            _input.Enable();
        }

        private void GoOption()
        {
            _clickSFX.Play();

            OptionPanel.gameObject.SetActive(true);
        }

        private void GoBackWithoutOption()
        {
            _clickSFX.Play();

            OptionPanel.gameObject.SetActive(false);
        }
        
        private void ReplaceMenu(bool isMenu)
        {
            HudPanel.gameObject.SetActive(!isMenu);
        }
    }
}