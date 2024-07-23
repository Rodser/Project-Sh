﻿using System.Collections.Generic;
using DG.Tweening;
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

        public async void ToDestroy()
        {
            if(GroundType == GroundType.Hole || GroundType == GroundType.Pit)
                return;
            
            if(Presenter.View == null)
                return;
            
            var tr = Presenter.View.transform;
            var tween = tr.DOMoveY(tr.position.y - 5, 0.7f);
            await tween.AsyncWaitForCompletion();
            Object.Destroy(Presenter.View.gameObject);
            GroundType = GroundType.Pit;

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