using UnityEngine;

namespace Shudder.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "HeroSFXConfig", menuName = "Game/HeroSFXConfig", order = 14)]
    public class HeroSfxConfig : ScriptableObject
    {
        [field: SerializeField] public AudioSource BoomSFX { get; private set; } = null;
        [field: SerializeField] public AudioSource JumpSFX { get; private set; } = null;
        [field: SerializeField] public AudioSource TakeSFX { get; private set; } = null;
        [field: SerializeField] public AudioSource PortalSFX { get; private set; } = null;
    }
}