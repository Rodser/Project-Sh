using UnityEngine;

namespace Shudder.Root
{
    public class Boot : MonoBehaviour
    {
        private static GameEntryPoint _instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AutostartGame()
        {
            SetSystemSettings();
            
            _instance = new GameEntryPoint();
            _instance.RunGame();
        }

        private static void SetSystemSettings()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}