using Shudder.Configs;
using Shudder.Gameplay.Views;
using Shudder.Models;
using UnityEngine;
using Grid = Shudder.Models.Grid;

namespace Shudder.Gameplay.Factories
{
    public class JewelKeyFactory
    {
        public JewelKeyView Create(JewelKeyView jewelKeyView, GridConfig config, Grid grid)
        {
            var parent = GetParent(grid, config);
            
            return Object.Instantiate(jewelKeyView, parent.AnchorPoint);
        }

        private Ground GetParent(Grid grid, GridConfig config)
        {
            var position = CalculatePosition(config);
            var parent = grid.Grounds[position.x, position.y];

            if (parent.GroundType is GroundType.Pit
                || parent.GroundType is GroundType.Portal
                || parent.GroundType is GroundType.Wall)
            {
                return GetParent(grid, config);
            }

            parent.IsStatic = true;
            
            return parent;
        }

        private Vector2Int CalculatePosition(GridConfig config)
        {
            var x = Random.Range(config.KeyPositionForWidth.x, config.KeyPositionForWidth.y);
            var y = Random.Range(config.KeyPositionForHeight.x, config.KeyPositionForHeight.y);
            return new Vector2Int(x, y);
        }
    }
}