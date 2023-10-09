using UnityEngine;

namespace Logic
{
    public class LightFactory
    {
        public Light Create(Light light, Transform cameraTransform, Transform bodyTransform)
        {
            return Object.Instantiate(light, cameraTransform.position, cameraTransform.rotation, bodyTransform);
        }
    }
}