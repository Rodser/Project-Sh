using UnityEngine;

namespace Shudder.Gameplay.Views
{
    public class PortalView : MonoBehaviour
    {
        [SerializeField] private GameObject portal;

        public void HidePortal()
        {
            portal.SetActive(false);
        }

        public void ShowPortal()
        {
            portal.SetActive(true);
        }
    }
}