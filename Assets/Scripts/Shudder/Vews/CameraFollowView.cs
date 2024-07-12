using UnityEngine;

namespace Shudder.Vews
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        public Camera Camera => _camera;
    }
}