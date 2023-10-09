using Model;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "Ground", menuName = "Game/Ground", order = 8)]
    public class GroundConfig : ScriptableObject
    {
        [field: SerializeField] public Ground Prefab { get; private set; } = null;
        [field: SerializeField, Header("Hole")]  public Ground PrefabHole { get; private set; } = null;
        [field: SerializeField, Header("Pit")]  public Ground PrefabPit { get; private set; } = null;
        
        [field: SerializeField, Header("Wall")]  public Ground PrefabWall { get; private set; } = null;
    }
}
