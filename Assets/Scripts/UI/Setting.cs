using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class Setting : MonoBehaviour
    {
        [field: SerializeField] public Button NextButton { get; private set; }
        
        [field: SerializeField] public Button BackButton { get; private set; }
        public void Subscribe(UnityAction goBackWithoutOption)
        {
            BackButton.onClick.AddListener(goBackWithoutOption);
        }
    }
}