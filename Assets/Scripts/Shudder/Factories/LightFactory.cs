using Shudder.Vews;
using UnityEngine;
using Grid = Shudder.Models.Grid;

namespace Shudder.Factories
{
    public class LightFactory
    {
        public void Create(LightPointView[] lights, Grid grid, float chance)
        {
            foreach (var ground in grid.Grounds)
            {
                if (!CanCreate(chance) || ground.Presenter.View == null) 
                    continue;

                var prefabLight = lights[Random.Range(0, lights.Length)];
                
                var position = ground.AnchorPoint.position;
                Object.Instantiate(prefabLight, position, Quaternion.identity, ground.AnchorPoint);
            }
        }

        private bool CanCreate(float chance)
        {
            var value = Random.value;
            return value < chance;
        }
    }
}