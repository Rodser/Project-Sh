using Shudder.Gameplay.Presenters;
using Shudder.Models.Interfaces;
using UnityEngine;

namespace Shudder.Gameplay.Models.Interfaces
{
    public interface IHero
    {
        HeroPresenter Presenter { get; set; }
        
        bool AtHole { get; set; }
        float Speed { get; set;}
        int Health { get; set;}
        IGround CurrentGround { get; set; }
        Vector3 Position { get; set; }

        void Damage();
        void ChangePosition(Vector3 position);
        void ChangeGround(IGround ground);
        void SetGround(IGround ground);
    }
}