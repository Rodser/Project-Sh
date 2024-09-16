using System.Collections.Generic;
using DG.Tweening;
using Shudder.Models.Interfaces;
using Shudder.Presenters;
using UnityEngine;

namespace Shudder.Models
{
    public class Ground : IGround
    {
        public Ground(Vector2Int groundId, GroundType groundType, Vector3 offsetPosition)
        {
            Id = groundId;
            GroundType = groundType;
            OffsetPosition = offsetPosition;
        }

        public Ground(GroundType groundType)
        {
            GroundType = groundType;
        }

        public Vector2Int Id { get; }
        public Transform AnchorPoint => Presenter.View.AnchorPoint;
        public GroundType GroundType { get; set; }
        public Vector3 OffsetPosition { get; }
        public List<Ground> Neighbors { get; set; }
        public GroundPresenter Presenter { get; set; }
        public bool IsStatic { get; set; }
        

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
            if(IsStatic)
                return;
            if(GroundType == GroundType.Portal || GroundType == GroundType.Pit)
                return;
            if(Presenter.View == null)
                return;
            
            GroundType = GroundType.Pit;

            var childs = Presenter.View.gameObject.GetComponentsInChildren<Transform>();
            for (int i = 0; i < childs.Length; i++)
            {
                Object.Destroy(childs[i].gameObject);
            }
        }
        
        public async void ToDestroyAsync()
        {
            if(IsStatic)
                return;
            if(GroundType == GroundType.Portal || GroundType == GroundType.Pit)
                return;
            if(Presenter.View == null)
                return;
            
            GroundType = GroundType.Pit;
            var tr = Presenter.View.transform;
            var tween = tr.DOMoveY(tr.position.y - 5, 0.7f);
            await tween.AsyncWaitForCompletion();

            var childs = Presenter.View.gameObject.GetComponentsInChildren<Transform>();
            for (int i = 0; i < childs.Length; i++)
            {
                Object.Destroy(childs[i].gameObject);
            }

            for (var n = 0; n < Neighbors.Count; n++)
            {
                var neighbor = Neighbors[n];
               
                for (var i = 0; i < neighbor.Neighbors.Count; i++)
                {
                    if (neighbor.Neighbors[i].Id != Id)
                        continue;
                    neighbor.Neighbors.Remove(neighbor.Neighbors[i]);
                }
            }
        }
    }
}