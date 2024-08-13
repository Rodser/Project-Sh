using UnityEngine;

namespace Shudder.Gameplay.Views
{
    public class PortalView : MonoBehaviour
    {
        [SerializeField] private GameObject portal;
        [SerializeField] private GameObject sphera;
        [SerializeField] private Collider portalCollider;

        public void HidePortal()
        {
            portal.SetActive(false);
            sphera.SetActive(true);
        }

        public void ShowPortal()
        {
            portal.SetActive(true);
            sphera.SetActive(false);
        }

        public void OffCollider()
        {
            sphera.SetActive(false);
            portalCollider.gameObject.SetActive(false);
        }
    }
}