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
                if(CanCreate() && ground.Presenter.View != null)
                    Object.Instantiate(light, ground.AnchorPoint);
            }
        }

        private bool CanCreate()
        {
            var value = Random.value;
            return value > 0.7f;
        }
    }
}