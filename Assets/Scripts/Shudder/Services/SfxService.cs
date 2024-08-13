using BaCon;
using Config;
using Shudder.Events;
using Shudder.Extensions;
using Shudder.Gameplay.Configs;
using UnityEngine;

namespace Shudder.Services
{
    public class SfxService
    {
        private AudioSource _boom;
        private AudioSource _jump;
        private AudioSource _take;
        private AudioSource _portal;
        private AudioSource _music;
        private AudioSource _musicMenu;
        private AudioSource _currentMusic;
        private AudioSource _takeCoin;

        private float _soundMute = 1f;
        private float _musicMute = 1f;
        private readonly Transform _cameraTransform;

        public SfxService(DIContainer container)
        {
            _cameraTransform = container.Resolve<CameraService>().CameraFollow.Presenter.View.transform;
            var readOnlyEvent = container.Resolve<IReadOnlyEventBus>();
            
            readOnlyEvent.MusicMute.AddListener(OnMusicMute);
            readOnlyEvent.SoundMute.AddListener(OnSoundMute);
        }

        public float MusicMute
        {
            get
            {
                MusicOnOff(_musicMute);
                return _musicMute;
            }
            private set
            {
                MusicOnOff(value);
                _musicMute = value;
            }
        }

        public float SoundMute
        {
            get
            {
                SoundOnOff(_soundMute);
                return _soundMute;
            }
            private set
            {
                SoundOnOff(value);
                _soundMute = value;
            }
        }

        public void Thunder() => 
            _boom?.Play();

        public void Jump() => 
            _jump?.Play();

        public void Take() => 
            _take?.Play();

        public void TakeCoin() => 
            _takeCoin?.Play();
        
        public void InPortal() => 
            _portal?.Play();

        public void StartMusic() => 
            _currentMusic?.Play();

        public void StopMusic() => 
            _currentMusic?.Stop();

        public void CreateMusicMenu(SFXConfig config)
        {
            _musicMenu = _musicMenu.Create(config.Music, _cameraTransform);
            _currentMusic = _musicMenu;
            StartMusic();
        }

        public void CreateMusic(SFXConfig config)
        {
            _music = _music.Create(config.Music, _cameraTransform);
            _currentMusic = _music;
            StartMusic();
        }

        public void CreateHeroSfx(HeroSfxConfig heroSfxConfig)
        {
            _boom = _boom.Create(heroSfxConfig.BoomSFX, _cameraTransform);
            _jump = _jump.Create(heroSfxConfig.JumpSFX, _cameraTransform);
            _take = _take.Create(heroSfxConfig.TakeSFX, _cameraTransform);
            _takeCoin = _take.Create(heroSfxConfig.TakeCoinSFX, _cameraTransform);
            _portal = _portal.Create(heroSfxConfig.PortalSFX, _cameraTransform);
        }

        private void OnMusicMute(float value) => 
            MusicMute = value;

        private void OnSoundMute(float value) => 
            SoundMute = value;

        private void MusicOnOff(float value)
        {
            _currentMusic.SetMute(value);
        }

        private void SoundOnOff(float value)
        {
            _boom.SetMute(value);
            _jump.SetMute(value);
            _take.SetMute(value);
            _portal.SetMute(value);
        }
    }
}