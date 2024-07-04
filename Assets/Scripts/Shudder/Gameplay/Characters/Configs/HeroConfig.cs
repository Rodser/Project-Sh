using Shudder.Gameplay.Characters.Views;
using UnityEngine;

namespace Shudder.Gameplay.Characters.Configs
{
    [CreateAssetMenu(fileName = "Hero", menuName = "Game/Hero", order = 11)]
    public class HeroConfig : ScriptableObject
    {
        [field: SerializeField] public HeroView Prefab { get; private set; }
        [field: SerializeField] public int StartPositionX { get; private set; }
        [field: SerializeField] public int StartPositionY { get; private set; }
        [field: SerializeField] public int StartPositionZ { get; private set; }
        [field: SerializeField] public float SpeedMove { get; private set; }
    }
}