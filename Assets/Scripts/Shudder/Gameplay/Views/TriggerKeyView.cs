using DI;
using Shudder.Gameplay.Services;
using UnityEngine;

namespace Shudder.Gameplay.Views
{
    public class TriggerKeyView : MonoBehaviour
    {
        private ActivationPortalService _activationService;
        private bool _isActive;

        public void Activate(DIContainer container, bool isKey)
        {
            _isActive = isKey;
            _activationService = container.Resolve<ActivationPortalService>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!_isActive)
                return;
            
            var keyView = other.GetComponentInParent<JewelKeyView>();
            if (keyView is null)
                return;
            
            _activationService.HasTookKey();
            Destroy(keyView.gameObject);
        }
    }
}