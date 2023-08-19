using System;
using Cysharp.Threading.Tasks;
using Rodser.Model;
using UnityEngine;

namespace Model
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        private Vector3 _target;
        private Rigidbody _rigidbody;
        private float _speed;
        private Vector3 _startPosition;
        private NotifySystem _notifySystem;

        public bool AtHole { get; set; }

        private void Start()
        {
            _startPosition = transform.position;
        }

        internal void MoveToTargetAsync(Vector3 holePosition)
        {
            _target = holePosition;
            _rigidbody = GetComponent<Rigidbody>();

            MoveAsync();
        }

        internal void SetSpeed(float speedMove)
        {
            _speed = speedMove;
        }
        
        internal void SetSystem(NotifySystem notifySystem)
        {
            _notifySystem = notifySystem;
        }
        
        private async void MoveAsync()
        {
            await UniTask.Delay(500);
            while (!AtHole)
            {
                if(transform == null)
                    return;
                
                var force = _target - transform.position;
                _rigidbody.AddForce(force.normalized * _speed, ForceMode.Acceleration);
                await UniTask.Yield();
            }
        }

        private void ReachHole()
        {
            AtHole = false;
            transform.position = _startPosition;
            AtHole = true;
            MoveAsync();
        }

        private void OnTriggerEnter(Collider other)
        {
            Ground ground = other.GetComponentInParent<Ground>();
            if (ground == null)
                return;

            if (ground.GroundType == GroundType.Hole)
            {
                ReachHole();
                Debug.Log("Victory");
                _notifySystem.Notify(isVictory:true);
            }
            else if (ground.GroundType == GroundType.Pit)
            {
                ReachHole();
                Debug.Log("Looser");
                _notifySystem.Notify(isVictory:false);
            }
        }
    }
}