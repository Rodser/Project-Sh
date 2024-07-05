using System.Linq;
using Rodser.Config;
using UI;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "Game", menuName = "Game/Game", order = 6)]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public Object[] Configs { get; private set; } = null;
        
        [field: SerializeField] public HexogenGridConfig[] LevelGridConfigs { get; private set; } = null;
        [field: SerializeField] public GameObject Title { get; private set; } = null;
        [field: SerializeField] public Light Light{ get; private set; } = null;
        [field: SerializeField] public UserInterface UserInterface { get; private set; } = null;
        
        public T GetConfig<T>()
        {
            return Configs.Where(c => 
                c.GetType() == typeof(T)).Cast<T>().FirstOrDefault();
        }
    }
}