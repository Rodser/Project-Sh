using UnityEngine;

namespace Shudder.Extensions
{
    public static class AudioSourceExtension
    {
        public static AudioSource Create(this AudioSource source, AudioSource audioSourceInfo, Transform target)
        {
            if (source is null) 
                return Object.Instantiate(audioSourceInfo, target);
            
            source.gameObject.transform.SetParent(target);
            return source;
        }

        public static void SetMute(this AudioSource source, float value)
        {
            if(source is null)
                return;
            source.mute = !(value > 0.5f);
        }
    }
}