using Config;
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
        [field: SerializeField] public GridConfig MenuGridConfig { get; private set; } = null;
        [field: SerializeField] public LightPointView[] Lights{ get; private set; } = null;
        [field: SerializeField] public ItemView[] Items{ get; private set; } = null;
        [field: SerializeField] public SFXConfig SfxConfig { get; private set; } = null;
        
        [field: Space, Header("UI")]
        [field: SerializeField] public UIMenuView UIMenuView { get; private set; } = null;
        [field: SerializeField] public UISettingView UISettingView { get; private set; } = null;
        [field: SerializeField] public UIRewardWindowView UIRewardWindowView { get; private set; } = null;
        
        [field: Space, Header("Hero")]
        [field: SerializeField]public HeroConfig HeroConfig { get; private set; }
      
        [field: Space, Header("Camera")]
        [field: SerializeField] public Vector3 CameraPosition { get; private set; }
        [field: SerializeField] public Vector3 CameraRotation { get; private set; }
    }
}