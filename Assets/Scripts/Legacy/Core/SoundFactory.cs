using Config;
using UnityEngine;

namespace Core
{
    public class SoundFactory
    {
        private SFXConfig _sFXConfig;

        public SoundFactory(SFXConfig sFXConfig)
        {
            _sFXConfig = sFXConfig;
        }

        public AudioSource Create(SFX sFX)
        {
            var prefab = _sFXConfig.Music;

            switch (sFX)
            {
                case SFX.Music:
                    break;
                case SFX.Click:
                    prefab = _sFXConfig.ButtonSFX;
                    break;
                case SFX.Boom:
                    prefab = _sFXConfig.BoomSFX;
                    break;
                case SFX.Winner:
                    prefab = _sFXConfig.WinnerSFX;
                    break;
                case SFX.Looser:
                    prefab = _sFXConfig.LooserSFX;
                    break;
            }
            return Object.Instantiate(prefab);
        }

        public AudioSource Create(Transform parent, SFX sFX = SFX.Boom)
        {
            var prefab = _sFXConfig.BoomSFX;
            return Object.Instantiate(prefab, parent);
        }
    }

    public enum SFX
    {
        Music = 1,
        Click = 2,
        Boom = 3,
        Winner = 4,
        Looser = 5
    }
}