using Shudder.Configs;
using Shudder.Gameplay.Configs;
using Shudder.UI;
using Shudder.Vews;
using UnityEngine;

namespace Shudder.MainMenu.Configs
{
    [CreateAssetMenu(fileName = "Menu", menuName = "Game/Menu", order = 12)]
    public class MenuConfig : ScriptableObject
    {
        [field: SerializeField] public HexogenGridConfig MenuGridConfig { get; private set; } = null;
        [field: SerializeField] public GameObject Title { get; private set; } = null;
        [field: SerializeField] public LightPointView Light{ get; private set; } = null;
        [field: SerializeField] public UIMenuView UIMenuView { get; private set; } = null;

        [field: Space, Header("Hero in Menu")]
        [field: SerializeField]public HeroConfig HeroConfig { get; private set; }
      
        [field: Space, Header("Camera")]
        [field: SerializeField] public Vector3 CameraPosition { get; private set; }
        [field: SerializeField] public Vector3 CameraRotation { get; private set; }
    }
}