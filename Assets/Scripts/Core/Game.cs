using System;
using Config;
using Cysharp.Threading.Tasks;
using DI;
using Logic;
using Model;
using Shudder.Gameplay.Characters.Factoryes;
using Shudder.Gameplay.Services;
using Shudder.UI;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class Game
    {
        private InputService _input;
        private UserInterface _userInterface;
        private CoinSystem _coinSystem;
        private HexogenGrid _currentGrid;
        private BodyGrid _body;
        private Camera _camera;
        private GameConfig _gameConfig;
        private DIContainer _container;
        
        private int _currentLevel = 0;
        private NotifySystem _notifySystem;

        public event Action<int, bool> ChangeHealth;
        public event Action<int> ChangeCoin;

        public void Run(DIContainer container, GameConfig gameConfig)
        {
            _container = container;
            _gameConfig = gameConfig;
        
            _input = _container.Resolve<InputService>();
            _camera = Camera.main;
            StartLevelAsync();
        }

        private async void StartLevelAsync()
        {
            await LoadLevelAsync(_currentLevel);
        }

        private async UniTask LoadLevelAsync(int level)
        {
            Debug.Log($"Load Level {level}");
            if (_body != null)
                _body.Did();

            _input.Clear();

            _body =_container.Resolve<BodyFactory>().Create();
            _container.Resolve<LightFactory>().Create(_gameConfig.Light, _camera.transform, _body.transform);

            _currentGrid = await _container.Resolve<GridFactory>("LevelGrid").Create(level, _body.transform);
            //var boomSFX = _container.Resolve<SoundFactory>().Create(SFX.Boom);

            var heroView = _container.Resolve<HeroFactory>().Create(_currentGrid.OffsetPosition, level, _body.gameObject);
            var moveService = _container.Resolve<HeroMoveService>();
            moveService.Subscribe(heroView);
        }

        private void OnNotify(bool isVictory)
        {
            if (!isVictory)
                return;

            _coinSystem.Change();
                                    
            if(_currentLevel + 1 < _gameConfig.LevelGridConfigs.Length)
                _currentLevel++;
        }
    }
}