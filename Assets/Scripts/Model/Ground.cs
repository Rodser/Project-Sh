using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;

namespace Model
{
    public class Ground : MonoBehaviour
    {
        [SerializeField] private Transform _vfxTransform = null;

        private float _height = 0;
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

        internal void Set(Vector2 id, GroundType groundType)
        {
            Id = id;
            GroundType = groundType;
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

        private void Lift()
        {
            Raise(GroundType == GroundType.TileHigh);
            _height = (int)GroundType * 0.5f + _offset;
            MoveAsync();
        }

        private async void MoveAsync()
        {
            if(gameObject == null)
                return;
            
            var target = transform.position;
            target.y = _height;

            var time = 0f;
            while (time < 1)
            {
                if(transform == null)
                    return;
                
                await UniTask.Yield();
                time += 1f * Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, target, time);
            }
        }

        private void Swap()
        {
            if (GroundType == GroundType.Pit || GroundType == GroundType.Hole || GroundType == GroundType.Wall)
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

        private void CreateVFX(VisualEffect vfx)
        {
            Instantiate(vfx, _vfxTransform);
        }
    }
}