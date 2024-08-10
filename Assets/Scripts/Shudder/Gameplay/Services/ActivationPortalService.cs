using Shudder.Gameplay.Views;
using Shudder.Models;

namespace Shudder.Gameplay.Services
{
    public class ActivationPortalService
    {
        private Grid _currentGrid;

        public void Construct(Grid currentGrid, bool isKey)
        {
            _currentGrid = currentGrid;
            _currentGrid.ActivatePortal(!isKey);
        }

        public void HasTookKey()
        {
            _currentGrid.ActivatePortal();
        }
    }
}