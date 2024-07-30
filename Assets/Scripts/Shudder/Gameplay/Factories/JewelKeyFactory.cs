using Shudder.Configs;
using Shudder.Gameplay.Views;
using Shudder.Models;
using UnityEngine;
using Grid = Shudder.Models.Grid;

namespace Shudder.Gameplay.Factories
{
    public class JewelKeyFactory
    {
        public JewelKeyView Create(JewelKeyView jewelKeyView, HexogenGridConfig config, Grid grid)
        {
            var parent = GetParent(grid, config);
            
            return Object.Instantiate(jewelKeyView, parent.AnchorPoint);
        }

        private Ground GetParent(Grid grid, HexogenGridConfig config)
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

        private Vector2Int CalculatePosition(HexogenGridConfig config)
        {
            var x = Random.Range(config.MinKeyPositionForX - 1, config.MaxKeyPositionForX);
            var y = Random.Range(config.MinKeyPositionForY - 1, config.MaxKeyPositionForY);
            return new Vector2Int(x, y);
        }
    }
}