using Cysharp.Threading.Tasks;
using Shudder.Gameplay.Presenters;
using Shudder.Models.Interfaces;

namespace Shudder.Gameplay.Models.Interfaces
{
    public interface IHero
    {
        HeroPresenter Presenter { get; set; }
        IGround CurrentGround { get; set; }

        UniTask ChangeGround(IGround ground);
        void SetGround(IGround ground);
    }
}