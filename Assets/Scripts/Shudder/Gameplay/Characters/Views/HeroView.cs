using Model;
using Shudder.Gameplay.Characters.Models;
using UnityEngine;

namespace Shudder.Gameplay.Characters.Views
{
    [RequireComponent(typeof(Rigidbody))]
    public class HeroView : MonoBehaviour
    {
        private Vector3 _startPosition;
        public Hero Hero;
        
        public Rigidbody Rigidbody { get; private set; }

        private void Start()
        {
            _startPosition = transform.position;
            Rigidbody = GetComponent<Rigidbody>();
        }

        public void Construct(Hero hero)
        {
            Hero = hero;
        }
        
        private void ResetBallPosition()
        {
            Hero.Damage();
            Debug.Log(Hero.Health);
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
                    Hero.AtHole = true;
                    Debug.Log("Victory");
                    Destroy(gameObject, 1000);
                    break;
                case GroundType.Pit when Hero.Health > 0:
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