using UnityEngine;

namespace Shudder.Services
{
    public class RotationService
    {
        public void LookRotation(Transform transform, Vector3 direction)
        {
            direction.y = transform.position.y;
            var relativePos = direction - transform.position;
            var toRotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation =  toRotation;            
        }
    }
}