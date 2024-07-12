using Shudder.Gameplay.Models;
using Shudder.Models;

namespace Shudder.Gameplay.Services
{
    public class CheckingPossibilityOfJumpService
    {
        public bool CheckPossible(GroundType select, GroundType hero)
        {
            int s = (int)select;
            int h = (int)hero;
            if (s == h)
                return true;
            if (s == h + 1)
                return true;
            if (s == h - 1)
                return true;
            return false;
        }
    }
}