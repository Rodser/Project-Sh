using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "SoundFX", menuName = "Game/SoundFX", order = 10)]
    public class SFXConfig : ScriptableObject
    {
        [field: SerializeField] public AudioSource ButtonSFX { get; private set; } = null;
       [field: SerializeField] public AudioSource VinnerSFX { get; private set; } = null;
        [field: SerializeField] public AudioSource LooserSFX { get; private set; } = null;
        [field: SerializeField] public AudioSource Music { get; private set; } = null;
    }
}