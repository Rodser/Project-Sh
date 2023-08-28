using Model;
using UnityEngine;
using UnityEngine.VFX;

namespace Rodser.Config
{
    [CreateAssetMenu(fileName = "Ground", menuName = "Game/Ground", order = 8)]
    public class GroundConfig : ScriptableObject
    {
        [field: SerializeField] public Ground Prefab { get; private set; } = null;
        
        [field: SerializeField, Header("Hole")] public Material MaterialHole { get; private set; } = null;
        [field: SerializeField] public VisualEffect VFXHole { get; private set; } = null;

        [field: SerializeField, Header("Pit")] public Material MaterialPit { get; private set; } = null;
        [field: SerializeField] public VisualEffect VFXPIt { get; private set; } = null;
    }
}
