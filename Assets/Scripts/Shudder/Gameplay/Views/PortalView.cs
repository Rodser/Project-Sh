using UnityEngine;

namespace Shudder.Gameplay.Views
{
    public class PortalView : MonoBehaviour
    {
        [SerializeField] private GameObject portal;
        [SerializeField] private Collider portalCollider;

        public void HidePortal()
        {
            portal.SetActive(false);
        }

        public void ShowPortal()
        {
            portal.SetActive(true);
        }

        public void OffCollider()
        {
            portalCollider.gameObject.SetActive(false);
        }
    }
}