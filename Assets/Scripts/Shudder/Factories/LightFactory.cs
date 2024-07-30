using Shudder.Vews;
using UnityEngine;
using Grid = Shudder.Models.Grid;

namespace Shudder.Factories
{
    public class LightFactory
    {
        public void Create(LightPointView light, Grid grid)
        {
            foreach (var ground in grid.Grounds)
            {
                if (!CanCreate() || ground.Presenter.View == null) 
                    continue;
                
                var position = ground.AnchorPoint.position;
                Object.Instantiate(light, position, Quaternion.identity, grid.Presenter.View.transform);
            }
        }

        private bool CanCreate()
        {
            var value = Random.value;
            return value > 0.8f;
        }
    }
}