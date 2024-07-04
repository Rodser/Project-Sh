using Cysharp.Threading.Tasks;
using Model;
using Shudder.Gameplay.Characters.Models;
using UnityEngine;

namespace Shudder.Gameplay.Characters.Views
{
    [RequireComponent(typeof(Rigidbody))]
    public class HeroView : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private Vector3 _startPosition;
        private Hero _hero;

        private void Start()
        {
            _startPosition = transform.position;
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Construct(Hero hero)
        {
            _hero = hero;
        }
        
        private async void MoveAsync(Vector3 position)
        {
            if(gameObject == null)
                 Destroy(this);
            
            await UniTask.Delay(150);
            var force = position - transform.position;
            _rigidbody.AddForce(force.normalized * _hero.Speed, ForceMode.Impulse);
        }

        private void ResetBallPosition()
        {
            _hero.Damage();
            Debug.Log(_hero.Health);
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
                    _hero.AtHole = true;
                    Debug.Log("Victory");
                    Destroy(gameObject, 1000);
                    break;
                case GroundType.Pit when _hero.Health > 0:
                    ResetBallPosition();
                    break;
                case GroundType.Pit:
                    Destroy(gameObject, 1000);
                    Debug.Log("Looser");
                    break;
            }
        }
    }
}