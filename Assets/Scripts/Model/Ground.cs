using Cysharp.Threading.Tasks;
using Rodser.Config;
using System.Collections.Generic;
using UnityEngine;

namespace Rodser.Model
{
    public class Ground : MonoBehaviour
    {        
        private int _height = 0;
        private GroundConfig _groundConfig = null;

        public Vector2 Id { get; private set; }
        public GroundType GroundType { get; private set; }
        public bool Raised { get; private set; }
        public List<Ground> Neighbors { get; private set; }

        public void Lift(int height)
        {            
            _height = height;
            Raise(height > 0);
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

        internal void Set(Vector2 id, GroundConfig groundConfig)
        {
            Id = id;
            _groundConfig = groundConfig;
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
            Lift((_height + 1) % 2);
        }

        private void Raise(bool raised)
        {
            Raised = raised;
        }

        public void AppointPit()
        {
            GroundType = GroundType.Pit;
            GetComponentInChildren<Collider>().isTrigger = true;
            GetComponentInChildren<MeshRenderer>().material = _groundConfig.MaterialPit;
            // set effect

        }

        internal void AppointHole()
        {
            GroundType = GroundType.Hole;
            GetComponentInChildren<Collider>().isTrigger = true;
            GetComponentInChildren<MeshRenderer>().material = _groundConfig.MaterialHole;
            // set effect
        }
    }
}