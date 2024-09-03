using Shudder.Models;
using Shudder.Views;
using UnityEngine;
using Grid = Shudder.Models.Grid;

namespace Shudder.Factories
{
    public class ItemFactory
    {
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
                if(ground.Id is { x: 1, y: 0 })
                    return;
                
                checkCount++;
                Object.Instantiate(item, ground.AnchorPoint);
            }
        }
        
        private bool CanCreate(float chance)
        {
            var value = Random.value;
            return value < chance;
        }
    }
}