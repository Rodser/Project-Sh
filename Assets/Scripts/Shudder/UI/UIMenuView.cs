using Shudder.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Shudder.UI
{
    public class UIMenuView : MonoBehaviour
    {
        private ITriggerOnlyEventBus _eventBus;
        
        [field: SerializeField] public Button StartButton { get; private set; }    
        [field: SerializeField] public Button BackButton { get; private set; }    
        [field: SerializeField] public Button OptionButton { get; private set; }    
        [field: SerializeField] public Button ExitButton { get; private set; }

        private void Start()
        {
            BackButton.gameObject.SetActive(false);
        }

        public void Bind(ITriggerOnlyEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void StartGame()
        {
            _eventBus.TriggerFlyCamera();
            gameObject.SetActive(false);
        }

        public void ExitGame()
        {
            _eventBus.TriggerExitGame();
        }

        public void ActivateBackButton()
        {
            BackButton.gameObject.SetActive(true);
        }
    }
}