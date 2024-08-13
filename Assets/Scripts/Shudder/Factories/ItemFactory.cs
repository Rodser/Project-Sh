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
            var prefabItem = items[Random.Range(0, items.Length)];
            Create(prefabItem, grid, grid.Grounds.Length, chance);
        }

        public void Create(ItemView item, Grid grid, int count, float chance)
        {
            var checkCuont = 0;
            foreach (var ground in grid.Grounds)
            {
                if(checkCuont >= count)
                    return;
                if (!CanCreate(chance)
                    || ground.Presenter.View == null
                    || ground.GroundType == GroundType.Pit
                    || ground.GroundType == GroundType.Portal) 
                    continue;
                
                checkCuont++;
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