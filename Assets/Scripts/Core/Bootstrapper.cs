using Rodser.Config;
using Rodser.Logic;
using UnityEngine;

namespace Rodser.Core
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private HexGridConfig hexGridConfig = null;

        private async void Awake()
        {
            GridFactory gridFactory = new GridFactory();
            await gridFactory.Create(hexGridConfig);
        }
    }
}