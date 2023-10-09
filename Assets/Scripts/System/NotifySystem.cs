using Model;
using UI;

namespace System
{
    internal class NotifySystem
    {
        private readonly UserInterface _userInterface;

        public NotifySystem(Ball ball, UserInterface userInterface)
        {
            _userInterface = userInterface;
            ball.SetSystem(this);
        }

        public void Notify(bool isVictory)
        {
            _userInterface.Notify(isVictory);
        }
    }
}