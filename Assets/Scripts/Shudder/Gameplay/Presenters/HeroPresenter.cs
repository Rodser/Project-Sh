using Shudder.Gameplay.Models.Interfaces;
using Shudder.Gameplay.Views;

namespace Shudder.Gameplay.Presenters
{
    public class HeroPresenter
    {
        public HeroPresenter(IHero hero)
        {
            Hero = hero;
        }

        public IHero Hero { get; }
        public HeroView View { get; set; }

        public void SetView(HeroView heroView)
        {
            View = heroView;
            SetPresenter();
        }

        private void SetPresenter()
        {
            Hero.Presenter = this;
        }
    }
}