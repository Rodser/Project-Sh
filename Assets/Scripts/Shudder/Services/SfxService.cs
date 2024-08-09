using BaCon;
using Config;
using Shudder.Factories;
using Shudder.Gameplay.Configs;
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

        public SfxService(DIContainer container)
        {
            _soundFactory = new SoundFactory();
        }

        public void Thunder() => 
            _boom.Play();

        public void Jump() => 
            _jump.Play();

        public void Take() => 
            _take.Play();

        public void InPortal() => 
            _portal.Play();

        public void StartMusic() => 
            _music.Play();

        public void StartMusicMenu() => 
            _musicMenu.Play();

        public void CreateMusic(SFXConfig config, Transform target) => 
            _music = _soundFactory.Create(config.Music, target);

        public void CreateHeroSfx(HeroSfxConfig heroSfxConfig, Transform hero)
        {
            _boom =  _soundFactory.Create(heroSfxConfig.BoomSFX, hero);
            _jump =  _soundFactory.Create(heroSfxConfig.JumpSFX, hero);
            _take =  _soundFactory.Create(heroSfxConfig.TakeSFX, hero);
            _portal =  _soundFactory.Create(heroSfxConfig.PortalSFX, hero);
        }

        public void CreateMusicMenu(SFXConfig config, Transform target) => 
            _musicMenu = _soundFactory.Create(config.Music, target);
    }
}