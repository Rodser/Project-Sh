using System.Collections.Generic;
using Shudder.Models.Interfaces;
using Shudder.Presenters;
using UnityEngine;

namespace Shudder.Models
{
    public class Ground : IGround
    {
        public Ground(Vector2 groundId, GroundType groundType, Vector3 offsetPosition)
        {
            Id = groundId;
            GroundType = groundType;
            OffsetPosition = offsetPosition;
        }

        public Ground(GroundType groundType)
        {
            GroundType = groundType;
        }

        public Vector2 Id { get; }
        public Transform AnchorPoint => Presenter.View.AnchorPoint;
        public GroundType GroundType { get; set; }
        public Vector3 OffsetPosition { get; }
        public List<Ground> Neighbors { get; set; }
        public GroundPresenter Presenter { get; set; }

        internal void AddNeighbors(List<Ground> neighbors)
        {
            Neighbors = neighbors;
        }

        public void ChangeGroundType(GroundType groundType)
        {
            GroundType = groundType;
        }

        public void ToDestroy()
        {
            if(GroundType == GroundType.Hole || GroundType == GroundType.Pit)
                return;
            
            if(Presenter.View == null)
                return;
            
            Object.Destroy(Presenter.View.gameObject);
            GroundType = GroundType.Pit;
        }
    }
}