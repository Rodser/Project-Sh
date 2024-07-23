using System.Collections.Generic;
using Shudder.Gameplay.Models;
using Shudder.Presenters;
using UnityEngine;

namespace Shudder.Models.Interfaces
{
    public interface IGround
    {
        Vector2 Id { get; }
        Transform AnchorPoint { get; }
        Vector3 OffsetPosition { get; }
        GroundType GroundType { get; set; }
        List<Ground> Neighbors { get; set; }
        GroundPresenter Presenter { get; set; }
        void ChangeGroundType(GroundType groundType);
        void ToDestroy();
    }
}