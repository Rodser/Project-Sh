using DI;
using Shudder.Gameplay.Services;
using Shudder.Services;
using UnityEngine;

namespace Shudder.Gameplay.Views
{
    public class TriggerKeyView : MonoBehaviour
    {
        private ActivationPortalService _activationService;
        private bool _isActive;
        private SfxService _sfx;

        public void Activate(DIContainer container, bool isKey)
        {
            _isActive = isKey;
            _activationService = container.Resolve<ActivationPortalService>();
            _sfx = container.Resolve<SfxService>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!_isActive)
                return;
            
            var keyView = other.GetComponentInParent<JewelKeyView>();
            if (keyView is null)
                return;
            
            _sfx.Take();
            _activationService.HasTookKey();
            Destroy(keyView.gameObject);
        }
    }
}