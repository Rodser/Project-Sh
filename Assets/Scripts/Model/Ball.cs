using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Rodser.Model
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        private Vector3 _target;
        private Rigidbody _rigidbody;
        private float _speed;

        public bool AtHole { get; set; }

        internal async void MoveToTargetAsync(Vector3 holePosition)
        {
            _target = holePosition;
            _rigidbody = GetComponent<Rigidbody>();
            
            await UniTask.Delay(500);
            await MoveAsync();
        }

        internal void SetSpeed(float speedMove)
        {
            _speed = speedMove;
        }

        private async UniTask MoveAsync()
        {
            while (!AtHole)
            {
                var force = _target - transform.position;
                _rigidbody.AddForce(force.normalized * _speed, ForceMode.Acceleration);
                await UniTask.Yield();
            }
        }

        private void ReachHole()
        {
            AtHole = false;
            Debug.Log("Victory");
        }

        private void OnTriggerEnter(Collider other)
        {
            Ground ground = other.GetComponentInParent<Ground>();
            if (ground == null) 
                return; 
        
            if(ground.GroundType == GroundType.Hole)
            {
                ReachHole();
            }
            else if (ground.GroundType == GroundType.Pit)
            {
                AtHole = false;
                Debug.Log("Loozzer");
            }
        }
    }
}