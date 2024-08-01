using UnityEngine;
using Object = UnityEngine.Object;

namespace Shudder.Factories
{
    public class SoundFactory
    {
        public AudioSource Create(AudioSource sfx, Transform parent)
        {
            return Object.Instantiate(sfx, parent);
        }
    }
}