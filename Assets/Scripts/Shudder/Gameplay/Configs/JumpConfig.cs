using UnityEngine;

namespace Shudder.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "Jump", menuName = "Game/Jump", order = 14)]
    public class JumpConfig : ScriptableObject
    {
        [field: SerializeField] public float DurationJump { get; private set; }
        [field: SerializeField] public float PowerJump { get; private set; }
        [field: SerializeField] public float Delay { get; private set; }
    }
}