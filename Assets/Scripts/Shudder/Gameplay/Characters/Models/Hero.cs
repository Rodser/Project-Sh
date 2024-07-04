namespace Shudder.Gameplay.Characters.Models
{
    public class Hero
    {
        public bool AtHole { get; set; }
        public float Speed { get; private set;}
        public int Health { get; private set;}

        public Hero(int health, float speed)
        {
            Health = health;
            Speed = speed;
            AtHole = false;
        }
        
        public void Damage()
        {
            Health--;
        }
    }
}