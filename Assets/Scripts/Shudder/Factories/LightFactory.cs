using Shudder.Vews;
using UnityEngine;
using Grid = Shudder.Models.Grid;

namespace Shudder.Factories
{
    public class LightFactory
    {
        public void Create(LightPointView[] lights, Grid grid)
        {
            foreach (var ground in grid.Grounds)
            {
                if (!CanCreate() || ground.Presenter.View == null) 
                    continue;

                var prefabLight = lights[Random.Range(0, lights.Length)];
                
                var position = ground.AnchorPoint.position;
                Object.Instantiate(prefabLight, position, Quaternion.identity, ground.AnchorPoint);
            }
        }

        private bool CanCreate()
        {
            var value = Random.value;
            return value > 0.9f;
        }
    }
}