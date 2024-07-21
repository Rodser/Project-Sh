using Shudder.Gameplay.Presenters;
using Shudder.Models.Interfaces;

namespace Shudder.Gameplay.Models.Interfaces
{
    public interface IHero
    {
        HeroPresenter Presenter { get; set; }
        IGround CurrentGround { get; set; }
        float Speed { get; set;}
        int Health { get; set;}

        void Damage();
        void ChangeGround(IGround ground);
        void SetGround(IGround ground);
    }
}