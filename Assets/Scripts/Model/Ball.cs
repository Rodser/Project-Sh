using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Model
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        private Action<int, bool> _changeHealth;

        private Rigidbody _rigidbody;
        private float _speed;
        private Vector3 _startPosition;
        private NotifySystem _notifySystem;
        private int _health;

        public bool AtHole { get; private set; }

        private void Start()
        {
            _startPosition = transform.position;
            _rigidbody = GetComponent<Rigidbody>();

            _health = 3;
        }

        internal void MoveToTargetAsync(Vector3 position)
        {
            MoveAsync(position);
        }

        internal void Construct(float speedMove, Action<int, bool> changeHealth)
        {
            _speed = speedMove;
            _changeHealth = changeHealth;
            _changeHealth?.Invoke(_health, false);
        }
        
        internal void SetSystem(NotifySystem notifySystem)
        {
            _notifySystem = notifySystem;
        }
        
        private async void MoveAsync(Vector3 position)
        {
            if(gameObject == null)
                 Destroy(this);
            
            await UniTask.Delay(150);
            var force = position - transform.position;
            _rigidbody.AddForce(force.normalized * _speed, ForceMode.Impulse);
        }

        private void ResetBallPosition()
        {
            _health--;
            _changeHealth?.Invoke(_health, true);
            Debug.Log(_health);
            transform.position = _startPosition;
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
                    Destroy(gameObject, 1000);
                    _notifySystem.Notify(isVictory:true);
                    break;
                case GroundType.Pit when _health > 0:
                    ResetBallPosition();
                    break;
                case GroundType.Pit:
                    Destroy(gameObject, 1000);
                    Debug.Log("Looser");
                    _notifySystem.Notify(isVictory:false);
                    break;
            }
        }
    }
}