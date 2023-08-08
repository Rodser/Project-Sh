using Cysharp.Threading.Tasks;
using Rodser.Config;
using System.Collections.Generic;
using UnityEngine;

namespace Rodser.Model
{
    public class Ground : MonoBehaviour
    {        
        private float _height = 0;
        private GroundConfig _groundConfig = null;

        public Vector2 Id { get; private set; }
        public GroundType GroundType { get; private set; }
        public bool Raised { get; private set; }
        public List<Ground> Neighbors { get; private set; }

        public void Lift()
        {
            Raise(GroundType == GroundType.TileHigh);
            _height = (int)GroundType * 0.5f;
            MoveAsync();
        }

        internal void AddNeighbors(List<Ground> neighbors)
        {
            Neighbors = neighbors;
        }

        internal async void SwapWaveAsync(List<Vector2> shifteds)
        {
            foreach (Vector2 item in shifteds)
            {
                if (item == Id) return;
            }

            shifteds.Add(Id);
            Swap();
            await UniTask.Delay(100);

            foreach (Ground ground in Neighbors)
            {
                ground.SwapWaveAsync(shifteds);
            }
        }

        internal void Set(Vector2 id, GroundConfig groundConfig, GroundType groundType)
        {
            Id = id;
            _groundConfig = groundConfig;
            GroundType = groundType;
            if (groundType == GroundType.Hole)
                AppointHole();
            else if(groundType == GroundType.Pit)
                AppointPit();
        }

        private async void MoveAsync()
        {
            var origin = transform.position;
            var target = origin;
            target.y = _height;

            var time = 0f;
            while (time < 1)
            {
                await UniTask.Yield();
                time += 1f * Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, target, time);
            }
        }

        private void Swap()
        {
            if (GroundType == GroundType.Pit || GroundType == GroundType.Hole)
                return;
            GroundType = GroundType - 1;
            if (GroundType < 0)
                GroundType = GroundType.TileHigh;
           
            Lift();
        }

        private void Raise(bool raised)
        {
            Raised = raised;
        }

        public void AppointPit()
        {
            GroundType = GroundType.Pit;
            GetComponentInChildren<Collider>().isTrigger = true;
            GetComponentInChildren<MeshRenderer>().materials[1] = _groundConfig.MaterialPit;
            // set effect

        }

        internal void AppointHole()
        {
            GroundType = GroundType.Hole;
            GetComponentInChildren<Collider>().isTrigger = true;
            GetComponentInChildren<MeshRenderer>().materials[0] = _groundConfig.MaterialHole;
            // set effect
        }
    }
}