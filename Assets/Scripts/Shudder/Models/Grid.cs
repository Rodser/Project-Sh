using Shudder.Gameplay.Views;
using Shudder.Models.Interfaces;
using Shudder.Presenters;

namespace Shudder.Models
{
    public class Grid : IGrid
    {
        public Ground[,] Grounds { get;  set; }
        public Ground Portal { get;  set; }
        public GridPresenter Presenter { get; set; }
        public int CountPit { get; set; }

        public void ActivatePortal(bool activate = true)
        {
            var portalView = Portal.Presenter.View.GetComponent<PortalView>();
            
            if(activate)
                portalView?.ShowPortal();
            else
                portalView?.HidePortal();
        }
        
        public void FreezePortal()
        {
            var portalView = Portal.Presenter.View.GetComponent<PortalView>();
            portalView.Freeze();
        }
    }
}