using UnityEngine;
using UnityEngine.UI;

namespace Shudder.UI
{
    public class LoadingScreenView : MonoBehaviour
    {
        [SerializeField] private Image loadingIndicator;

        private void Update()
        {
            loadingIndicator.transform.Rotate(0f, 0f, -100f* Time.deltaTime);
        }
    }
}