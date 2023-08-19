using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Rodser.Config;
using Rodser.Model;
using UnityEngine;
using UnityEngine.VFX;

namespace Model
{
    public class Ground : MonoBehaviour
    {
        [SerializeField] private Transform _vfxTransform = null;

        private float _height = 0;
        private GroundConfig _groundConfig = null;
        private float _offset;

        public Vector2 Id { get; private set; }
        public GroundType GroundType { get; private set; }
        public bool Raised { get; private set; }
        public List<Ground> Neighbors { get; private set; }

        public void Lift(float offset)
        {
            _offset = offset;
            Lift();
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

        private void Lift()
        {
            Raise(GroundType == GroundType.TileHigh);
            _height = (int)GroundType * 0.5f + _offset;
            MoveAsync();
        }

        private async void MoveAsync()
        {
            if(transform == null)
                return;
            
            var target = transform.position;
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

        private void AppointPit()
        {
            GroundType = GroundType.Pit;
            GetComponentInChildren<Collider>().isTrigger = true;
            GetComponentInChildren<MeshRenderer>().material = _groundConfig.MaterialPit;
            CreateVFX(_groundConfig.VFXPIt);

        }

        private void AppointHole()
        {
            GroundType = GroundType.Hole;
            GetComponentInChildren<Collider>().isTrigger = true;
            GetComponentInChildren<MeshRenderer>().material = _groundConfig.MaterialHole;
            CreateVFX(_groundConfig.VFXHole);
        }

        private void CreateVFX(VisualEffect vfx)
        {
            Instantiate(vfx, _vfxTransform);
        }
    }
}