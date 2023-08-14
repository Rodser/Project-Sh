using UnityEngine;
using UnityEngine.UI;

namespace Rodser.Core
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private Button _bStart;

        private void Awake()
        {
            _bStart.onClick.AddListener(StartLevel);
        }

        private void StartLevel()
        {
            Debug.Log("Start");
        }
    }
}