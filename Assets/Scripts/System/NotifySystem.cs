using Core;
using Model;

namespace System
{
    internal class NotifySystem
    {
        private readonly HUD _hud;

        public NotifySystem(Ball ball, HUD hud)
        {
            _hud = hud;
            ball.SetSystem(this);
        }

        public void Notify(bool isVictory)
        {
            _hud.Notify(isVictory);
        }
    }
}