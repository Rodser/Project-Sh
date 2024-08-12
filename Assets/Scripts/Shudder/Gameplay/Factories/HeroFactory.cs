using BaCon;
using Shudder.Gameplay.Configs;
using Shudder.Gameplay.Models;
using Shudder.Gameplay.Presenters;
using Shudder.Gameplay.Services;
using Shudder.Models;
using Shudder.Services;
using UnityEngine;

namespace Shudder.Gameplay.Factories
{
    public class HeroFactory
    {
        private readonly DIContainer _container;
        private readonly HeroConfig _heroConfig;

        public HeroFactory(DIContainer container, HeroConfig heroConfig)
        {
            _container = container;
            _heroConfig = heroConfig;
        }
        
        public Hero Create(Ground[,] grounds)
        {
            var ground = grounds[_heroConfig.StartPositionX, _heroConfig.StartPositionY];
            if (ground.GroundType == GroundType.Pit)
            {
                foreach (var ng in ground.Neighbors)
                {
                    if (ng.GroundType != GroundType.Pit)
                        ground = ng;
                }
            }

            var hero = new Hero(_container);
            var position = ground.AnchorPoint.position;
            var heroView = Object.Instantiate(_heroConfig.Prefab, position, Quaternion.identity, ground.AnchorPoint);
            var presenter = new HeroPresenter(hero);
            heroView.Construct(_container, presenter);
            hero.SetGround(ground);

            _container
                .Resolve<RotationService>()
                .LookRotation(hero.Presenter.View.transform, _container.Resolve<CameraService>().Camera.transform.position);
            _container.Resolve<AnimationHeroService>().SetAnimator(heroView.GetComponent<Animator>());
            _container.Resolve<SfxService>().CreateHeroSfx(_heroConfig.HeroSfxConfig);
            
            return hero;
        }
    }
}