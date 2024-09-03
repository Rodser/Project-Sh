using Shudder.Configs;
using Shudder.Constants;
using Shudder.Factories;
using Shudder.Gameplay.Services;
using Shudder.Presenters;
using Shudder.Services;
using Shudder.Views;
using UnityEngine;
using Grid = Shudder.Models.Grid;

namespace Utils
{
    public class BuildingTest
    {
        public void BuildGrid(GridConfig config)
        {
            var gridView = new GameObject("Grid").AddComponent<GridView>();

            var grid = new Grid();
            var presenter = new GridPresenter(grid);
            presenter.SetView(gridView);

            var buildInfo = Resources.Load<BuildInfo>(GameConstant.BuildInfoPath);
            var cameraFollow = new CameraFollowFactory().Instantiate(buildInfo.CameraFollowView);
            
            var builderGridService = new BuilderGridService();
            builderGridService.Construct(new CameraService(cameraFollow), new GroundFactory(new LiftService()));
            builderGridService
                .CreateGrounds(grid, config, false)
                .EstablishWall()
                .EstablishPit()
                .EstablishPortal()
                .Build();
        }
    }
}