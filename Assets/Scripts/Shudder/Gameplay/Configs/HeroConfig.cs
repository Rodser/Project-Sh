using Shudder.Gameplay.Views;
using UnityEngine;

namespace Shudder.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "Hero", menuName = "Game/Hero", order = 11)]
    public class HeroConfig : ScriptableObject
    {
        [field: SerializeField] public HeroView Prefab { get; private set; }
        [field: SerializeField] public JumpConfig JumpConfig { get; private set; }
        [field: SerializeField] public HeroSfxConfig HeroSfxConfig { get; private set; }
        [field: SerializeField] public int StartPositionX { get; private set; }
        [field: SerializeField] public int StartPositionY { get; private set; }
        [field: SerializeField] public float SpeedMove { get; private set; }
    }
}