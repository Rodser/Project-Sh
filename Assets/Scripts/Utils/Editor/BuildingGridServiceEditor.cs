using Shudder.Configs;
using Shudder.Factories;
using Shudder.Gameplay.Services;
using Shudder.Presenters;
using Shudder.Services;
using Shudder.Views;
using UnityEngine;
using Grid = Shudder.Models.Grid;

namespace Utils.Editor
{
    public class BuildingGridServiceEditor
    {
        public void BuildGrid(GridConfig config)
        {
            var gridView = new GameObject("Grid").AddComponent<GridView>();

            var grid = new Grid();
            var presenter = new GridPresenter(grid);
            gridView.Construct(presenter);

            var builderGridService = new BuilderGridService();
            builderGridService.Construct(new GroundFactory(new LiftService()));
            builderGridService
                .CreateGrounds(grid, config)
                .EstablishWall()
                .EstablishPit()
                .EstablishPortal()
                .Build();
        }
    }
}