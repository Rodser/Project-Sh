using Shudder.Gameplay.Views;
using UnityEngine;

namespace Shudder.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "Ground", menuName = "Game/Ground", order = 8)]
    public class GroundConfig : ScriptableObject
    {
        [field: SerializeField] public GroundView Prefab { get; private set; } = null;
        [field: SerializeField, Header("Hole")]  public GroundView PrefabHole { get; private set; } = null;
        [field: SerializeField, Header("Pit")]  public GroundView PrefabPit { get; private set; } = null;
        [field: SerializeField, Header("Wall")]  public GroundView PrefabWall { get; private set; } = null;
    }
}
