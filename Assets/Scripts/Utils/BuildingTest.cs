using Shudder.Configs;
using Shudder.Constants;
using Shudder.Factories;
using Shudder.Gameplay.Services;
using Shudder.Models;
using Shudder.Presenters;
using Shudder.Services;
using Shudder.Views;
using UnityEngine;

namespace Utils
{
    public class BuildingTest
    {
        var gridView = Object.FindFirstObjectByType<GridView>() ?? new GameObject("Grid").AddComponent<GridView>();

        var grid = new Shudder.Models.Grid();
        var presenter = new GridPresenter(grid);
        presenter.SetView(gridView);

        CameraFollow cameraFollow;
        var cameraFollowView = FindFirstObjectByType<CameraFollowView>();
            if (cameraFollowView is not null)
        {
            cameraFollow = cameraFollowView.Presenter.CameraFollow;
        }
    else
    {
        var buildInfo = Resources.Load<BuildInfo>(GameConstant.BuildInfoPath);;
        cameraFollow = new CameraFollowFactory().Create(buildInfo.CameraFollowView);
    }
             
    var builderGridService = new BuilderGridService(new CameraService(cameraFollow), new GroundFactory(new LiftService()));
    builderGridService
        .CreateGrounds(grid, this, false)
        .EstablishPit()
        .EstablishPortal()
        .Build();
    }
}