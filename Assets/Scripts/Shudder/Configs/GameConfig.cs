using System.Linq;
using Shudder.Gameplay.Models;
using Shudder.Gameplay.Views;
using Shudder.UI;
using Shudder.Views;
using UnityEngine;

namespace Shudder.Configs
{
    [CreateAssetMenu(fileName = "Game", menuName = "Game/Game", order = 6)]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public Object[] Configs { get; private set; } = null;
        [field: SerializeField] public GridConfig[] LevelGridConfigs { get; private set; } = null;
        [field: SerializeField] public Indicator SelectIndicator { get; private set; } = null;
        [field: SerializeField] public LightPointView[] Lights{ get; private set; } = null;
        [field: SerializeField] public ItemView[] Items{ get; private set; } = null;
        [field: SerializeField] public ItemView Coin { get; private set; } = null;
        [field: SerializeField] public JewelKeyView JewelKeyView { get; private set; }

        [field: Space, Header("UI")]
        [field: SerializeField] public HudView HudView { get; private set; }
        [field: SerializeField] public UISettingView UISettingView { get; private set; }
        [field: SerializeField] public UIVictoryWindowView UIVictoryWindowView { get; private set; }

        [field: Space, Header("Camera")]
        [field: SerializeField] public Vector3 CameraRotation { get; private set; }
        
        public T GetConfig<T>()
        {
            return Configs.Where(c => 
                c.GetType() == typeof(T)).Cast<T>().FirstOrDefault();
        }
    }
}