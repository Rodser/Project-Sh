using BaCon;
using Shudder.Models;
using Shudder.Services;
using Shudder.Views;
using UnityEngine;
using Grid = Shudder.Models.Grid;

namespace Shudder.Factories
{
    public class ItemFactory
    {
        private readonly CoinService _coinService;

        public ItemFactory(DIContainer container)
        {
            _coinService = container.Resolve<CoinService>();
        }

        public void Create(ItemView[] items, Grid grid, float chance)
        {
            foreach (var item in items)
            {
                Create(item, grid, grid.Grounds.Length, chance * 0.5f);
            }
        }

        public void Create(ItemView item, Grid grid, int count, float chance)
        {
            var checkCount = 0;
            foreach (var ground in grid.Grounds)
            {
                if(checkCount >= count)
                    return;
                if (!CanCreate(chance)
                    || ground.Presenter.View is null
                    || ground.GroundType == GroundType.Pit
                    || ground.GroundType == GroundType.Portal) 
                    continue;
                
                checkCount++;
                var view = Object.Instantiate(item, ground.AnchorPoint);
                if (view.TryGetComponent(out CoinView coinView))
                {
                    coinView.Construct(_coinService);
                }
            }
        }
        
        private bool CanCreate(float chance)
        {
            var value = Random.value;
            return value < chance;
        }
    }
}