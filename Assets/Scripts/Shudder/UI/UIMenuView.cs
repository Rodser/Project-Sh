using System;
using Shudder.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Shudder.UI
{
    public class UIMenuView : MonoBehaviour
    {
        [field: SerializeField] public Button StartButton { get; private set; }    
        [field: SerializeField] public Button BackButton { get; private set; }    
        [field: SerializeField] public Button OptionButton { get; private set; }    
        [field: SerializeField] public Button ExitButton { get; private set; }
        
        private EventBus _eventBus;

        private void Start()
        {
            BackButton.gameObject.SetActive(false);
        }

        public void StartGame()
        {
            _eventBus.TriggerStartGameplayScene();
            gameObject.SetActive(false);
        }

        public void ActivateBackButton()
        {
            BackButton.gameObject.SetActive(true);
        }

        public void Bind(EventBus eventBus)
        {
            _eventBus = eventBus;
        }
    }
}