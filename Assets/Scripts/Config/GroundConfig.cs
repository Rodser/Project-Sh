using Rodser.Model;
using UnityEngine;

namespace Rodser.Config
{
    [CreateAssetMenu(fileName = "Ground", menuName = "Game/Ground", order = 8)]
    public class GroundConfig : ScriptableObject
    {
        [field: SerializeField] public Ground Prefab { get; private set; } = null;
        [field: SerializeField] public Material MaterialPit { get; private set; } = null;
        [field: SerializeField] public Material MaterialHole { get; private set; } = null;
    }
}
