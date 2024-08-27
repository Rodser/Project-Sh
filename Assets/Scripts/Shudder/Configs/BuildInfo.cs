using Shudder.UI;
using Shudder.Views;
using UnityEngine;

namespace Shudder.Configs
{
    [CreateAssetMenu(fileName = "BuildInfo", menuName = "Game/BuildInfo", order = 0)]
    public class BuildInfo : ScriptableObject
    {
        [field: SerializeField] public UIRootView UIRootView { get; private set; } = null;
        [field: SerializeField] public CameraFollowView CameraFollowView { get; private set; } = null;

        public string BundleVersion { get; set; } = "0.5";

    }
}