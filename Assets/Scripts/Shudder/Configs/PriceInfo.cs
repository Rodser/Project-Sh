using UnityEngine;

namespace Shudder.Configs
{
    [CreateAssetMenu(fileName = "Price", menuName = "Game/Price", order = 20)]
    public class PriceInfo : ScriptableObject
    {
        [field: SerializeField]  public int Jump { get; private set; }
        [field: SerializeField]  public int Wave { get; private set; }
        [field: Space(7)]
        [field: SerializeField]  public int JumpX10 { get; private set; }
        [field: SerializeField]  public int WaveX10 { get; private set; }
        [field: Space(7)]
        [field: SerializeField]  public int SingleCoin { get; private set; }
        [field: SerializeField]  public int DefaultCache { get; private set; }
        [field: SerializeField]  public int RewardBonus { get; private set; }

    }
}