using BaCon;
using Config;
using JetBrains.Annotations;
using Shudder.Events;
using Shudder.Extensions;
using Shudder.Factories;
using Shudder.Gameplay.Configs;
using Shudder.Models;
using UnityEngine;

namespace Shudder.Services
{
    public class SfxService
    {
        private readonly SoundFactory _soundFactory;
        
        private AudioSource _boom;
        private AudioSource _jump;
        private AudioSource _take;
        private AudioSource _portal;
        private AudioSource _music;
        private AudioSource _musicMenu;
        private float _soundMute = 1f;
        private float _musicMute = 1f;
        private readonly Transform _cameraTransform;

        public SfxService(DIContainer container)
        {
            _soundFactory = new SoundFactory();
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

        private void OnMusicMute(float value) => 
            MusicMute = value;

        private void OnSoundMute(float value) => 
            SoundMute = value;

        private void MusicOnOff(float value)
        {
            _music.SetMute(value);
            _musicMenu.SetMute(value);
        }

        private void SoundOnOff(float value)
        {
            _boom.SetMute(value);
            _jump.SetMute(value);
            _take.SetMute(value);
            _portal.SetMute(value);
        }

        public void Thunder() => 
            _boom?.Play();

        public void Jump() => 
            _jump?.Play();

        public void Take() => 
            _take?.Play();

        public void InPortal() => 
            _portal?.Play();

        public void StartMusic() => 
            _music?.Play();

        public void StartMusicMenu() => 
            _musicMenu?.Play();

        public void CreateMusicMenu(SFXConfig config)
        {
            _musicMenu = _musicMenu.Create(config.Music, _cameraTransform);
        }

        public void CreateMusic(SFXConfig config)
        {
            _music =_music.Create(config.Music, _cameraTransform);
        }

        public void CreateHeroSfx(HeroSfxConfig heroSfxConfig)
        {
            _boom = _boom.Create(heroSfxConfig.BoomSFX, _cameraTransform);
            _jump = _jump.Create(heroSfxConfig.JumpSFX, _cameraTransform);
            _take = _take.Create(heroSfxConfig.TakeSFX, _cameraTransform);
            _portal = _portal.Create(heroSfxConfig.PortalSFX, _cameraTransform);
        }
    }
}