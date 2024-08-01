using Shudder.Models;
using Shudder.Vews;
using UnityEngine;
using Grid = Shudder.Models.Grid;

namespace Shudder.Factories
{
    public class ItemFactory
    {
        public void Create(ItemView[] items, Grid grid, float chance)
        {
            foreach (var ground in grid.Grounds)
            {
                if (!CanCreate(chance)
                    || ground.Presenter.View == null
                    || ground.GroundType == GroundType.Pit
                    || ground.GroundType == GroundType.Portal) 
                    continue;

                var prefabItem = items[Random.Range(0, items.Length)];
                
                Object.Instantiate(prefabItem, ground.AnchorPoint);
            }
        }

        private bool CanCreate(float chance)
        {
            var value = Random.value;
            return value < chance;
        }
    }
}