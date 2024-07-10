using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Shudder.Gameplay.Models
{
    public class Ground
    {
        public UnityEvent<Vector3> ChangePosition = new ();
        
        public Ground(Vector2 groundId, GroundType groundType, Transform anchorPoint)
        {
            Id = groundId;
            GroundType = groundType;
            AnchorPoint = anchorPoint;
        }

        public Vector2 Id { get; }
        public Transform AnchorPoint { get; }
        public GroundType GroundType { get; set; }
        public List<Ground> Neighbors { get; private set; }
        public Vector3 Position
        {
            get => Position;
            set
            {
                Position = value;
                ChangePosition?.Invoke(value);
            }
        }

        internal void AddNeighbors(List<Ground> neighbors)
        {
            Neighbors = neighbors;
        }
    }
}