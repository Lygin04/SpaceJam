using System;
using Event.Service;
using Game.Player.Service;
using Game.Spawn.Event;
using Game.UFO.Controller;
using UnityEngine;

namespace Game.Spawn
{
    public class SpawnService : IDisposable
    {
        private readonly LevelInfo _levelInfo;
        private readonly PlayerService _playerService;

        private int _indexWave;
        private int _indexBullet;
        private GameObject _gameRoot;
        private UFOController _currentEnemy;
        private EventDispatcher _eventDispatcher;

        public SpawnService(LevelInfo levelInfo, EventDispatcher eventDispatcher, PlayerService playerService)
        {
            _levelInfo = levelInfo;
            _eventDispatcher = eventDispatcher;
            _playerService = playerService;
            eventDispatcher.AddListener<RequestWaveEvent>(RequestWaveEvent.REQUEST_WAVE, OnWaveRequested);
        }

        public void Init(GameObject gameRoot)
        {
            _gameRoot = gameRoot;
        }

        private void OnWaveRequested(RequestWaveEvent requestWave)
        {
            if (requestWave.WaveIndex != -1)
            {
                _indexWave = requestWave.WaveIndex;
                _indexBullet = 0;
            }

            LevelInfo.DiscWaveSpawn discWaveSpawn = _levelInfo.GetSpawnInfo(_indexWave);
            
            _playerService.SetParams(discWaveSpawn.TimeSelectDirectionInSeconds, discWaveSpawn.TimeSelectPowerInSeconds,
                discWaveSpawn.MaxLeftAngle,
                discWaveSpawn.MaxRightAngle);
            if (_currentEnemy != null)
            {
                GameObject.Destroy(_currentEnemy.gameObject);
            }

            _currentEnemy = GameObject.Instantiate<UFOController>(discWaveSpawn.Prefab);
            _currentEnemy.SetUpParams(discWaveSpawn.ScoreForFull, discWaveSpawn.CountTargetsToTake,
                discWaveSpawn.Health, discWaveSpawn.MaxRightPosition, discWaveSpawn.MaxLeftPosition, 
                discWaveSpawn.isAxisX, discWaveSpawn.speed);
            _currentEnemy.transform.SetParent(_gameRoot.transform);
            _currentEnemy.transform.position = discWaveSpawn.UFOPosition;
            _indexWave++;
        }

        public void Dispose()
        {
            _indexWave = 0;
            _eventDispatcher.RemoveListener<RequestWaveEvent>(RequestWaveEvent.REQUEST_WAVE, OnWaveRequested);
        }
    }
}