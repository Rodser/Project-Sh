using Shudder.Gameplay.Presenters;
using Shudder.Models.Interfaces;

namespace Shudder.Gameplay.Models.Interfaces
{
    public interface IHero
    {
        HeroPresenter Presenter { get; set; }
        IGround CurrentGround { get; set; }

        void ChangeGround(IGround ground);
        void SetGround(IGround ground);
    }
}