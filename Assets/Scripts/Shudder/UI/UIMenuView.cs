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
        
        public void ActivateBackButton()
        {
            BackButton.gameObject.SetActive(true);
        }
    }
}