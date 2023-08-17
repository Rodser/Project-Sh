using UnityEngine;
using UnityEngine.UI;

namespace Rodser.Core
{
    public class HUD : MonoBehaviour
    {
        [field: SerializeField] public Button StartButton { get; private set; }    
        [field: SerializeField] public RectTransform PanelMainMenu  { get; private set; }


        private void Awake()
        {
            StartButton.onClick.AddListener(StartLevel);
        }

        private void StartLevel()
        {
            Debug.Log("Start");
            PanelMainMenu.gameObject.SetActive(false);
        }
    }
}