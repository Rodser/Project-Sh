using Shudder.Gameplay.Configs;
using Shudder.UI;
using UnityEngine;

namespace Shudder.MainMenu.Configs
{
    [CreateAssetMenu(fileName = "Menu", menuName = "Game/Menu", order = 12)]
    public class MenuConfig : ScriptableObject
    {
        [field: SerializeField] public HexogenGridConfig MenuGridConfig { get; private set; } = null;
        [field: SerializeField] public GameObject Title { get; private set; } = null;
        [field: SerializeField] public Light Light{ get; private set; } = null;
        [field: SerializeField] public UIMenuView UIMenuView { get; private set; } = null;
    }
}