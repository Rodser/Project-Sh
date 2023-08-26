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
        private int _countLive;

        public bool AtHole { get; set; }

        private void Start()
        {
            _startPosition = transform.position;
            _countLive = 2;
        }

        internal void MoveToTargetAsync(Vector3 position)
        {
            _target = position;
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
                if(gameObject == null)
                    return;
                
                var force = _target - transform.position;
                _rigidbody.AddForce(force.normalized * _speed, ForceMode.Acceleration);
                await UniTask.Yield();
            }
        }

        private void ReachHole()
        {
            _countLive--;
            Debug.Log(_countLive);
            AtHole = true;
            transform.position = _startPosition;
            AtHole = false;
            MoveAsync();
        }

        private void OnTriggerEnter(Collider other)
        {
            Ground ground = other.GetComponentInParent<Ground>();
            if (ground == null)
                return;

            switch (ground.GroundType)
            {
                case GroundType.Hole:
                    AtHole = true;
                    Debug.Log("Victory");
                    Destroy(gameObject, 300);
                    _notifySystem.Notify(isVictory:true);
                    break;
                case GroundType.Pit when _countLive > 0:
                    AtHole = true;
                    ReachHole();
                    break;
                case GroundType.Pit:
                    AtHole = true;
                    Destroy(gameObject, 300);
                    Debug.Log("Looser");
                    _notifySystem.Notify(isVictory:false);
                    break;
            }
        }
    }
}